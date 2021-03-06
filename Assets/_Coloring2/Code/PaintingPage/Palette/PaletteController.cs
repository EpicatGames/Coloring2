using System;
using System.Collections;
using Coloring2.Configs;
using Coloring2.DataServices;
using Coloring2.MainMenu.Settings;
using Coloring2.PaintingPage.SystemControls;
using Coloring2.PaintingPage.Tools;
using Coloring2.Popups;
using DG.Tweening;
using UnityEngine;

namespace Coloring2.PaintingPage.Palette
{
    public class PaletteController : MonoBehaviour, IPalette
    {
        public event Action<PaletteCell> CellSelected;
        
        [SerializeField] private Transform _view;
        [SerializeField] private CallPaletteButton _callPaletteBtn;
        [SerializeField] private PaletteCell[] _cells;

        public Action<PaletteCell> CellTapped;

        public IPaletteCell SelectedCell { get; private set; }

        private AppConfig _appConfig;
        private PaintToolsController _paintToolController;
        private PlayerPurchasesService _purchaseService;

        private void Awake()
        {
            _paintToolController = GetComponent<PaintToolsController>();
            _view.localScale = Vector3.zero;
        }
        
        private void OnDestroy()
        {
            if(!StartApplication.Initialized)
                return;

            _paintToolController.ToolSelected -= OnToolSelected;
            CellTapped -= OnCellTapped;
        }

        public void Hide()
        {
            _view.DOScale(0, 0.4f)
                .OnComplete(() => _view.gameObject.SetActive(false));
            _callPaletteBtn.PaletteOutsideClosed?.Invoke();
        }
        
        public void Show()
        {
            _view.gameObject.SetActive(true);
            _view.DOScale(1, 0.4f);
        }

        public void Init()
        {
            _purchaseService = ServicesManager.GetService<PlayerPurchasesService>();
            
            _appConfig = ServicesManager.GetService<ConfigsService>().GetConfig<AppConfig>();
            _paintToolController.ToolSelected += OnToolSelected;
            CellTapped += OnCellTapped;

            var allColorsPurchased = _purchaseService.HasAllColorsPurchased() ||
                                     _purchaseService.HasCategoryPurchased(Categories.full_version);
            for (var i = 0; i < _cells.Length; i++)
            {
                var cell = _cells[i];
                var locked = !allColorsPurchased && i > 5;
                cell.Init(CellTapped, locked);
                if (i == 0)
                    SelectCell(cell);
            }
        }

        private void SelectCell(PaletteCell cell)
        {
            if(SelectedCell == cell)
                return;

            SelectedCell?.Deselect();

            SelectedCell = cell;
            SelectedCell.Select();
            CellSelected?.Invoke(cell);
        }

        private void OnToolSelected(IPaintTool tool)
        {
            foreach (var cell in _cells)
                cell.UpdateState(tool.Type);
        }

        private void OnCellTapped(PaletteCell cell)
        {
            SoundsManager.PlayClick();
            if (cell.IsLocked)
            {
                var popup = ModalPopupsManager.ShowPopup<EnterBirthdayPopup>(_appConfig.EnterBirthdayPopupRef);
                popup.Closed += OnPopupClose;
                popup.Success += OnSettingsEnterBirthdaySuccess;
            }
            else
            {
                SelectCell(cell);
            }
        }

        private void OnSettingsEnterBirthdaySuccess(EnterBirthdayPopup popup)
        {
            popup.Success -= OnSettingsEnterBirthdaySuccess;
            SoundsManager.PlayCorrectBirthYear();

            ModalPopupsManager.ShowPopup(_appConfig.ColorsPurchasePopupRef);
            _purchaseService.Purchased += OnPurchased;
            ModalPopupsManager.Current.Closed += OnPopupClose;
        }

        private void OnPurchased(Categories category)
        {
            foreach (var cell in _cells)
            {
                if(cell.IsLocked)cell.Unlock();
            }
        }

        private void OnPopupClose(IPopup popup)
        {
            _purchaseService.Purchased -= OnPurchased;
            popup.Closed -= OnPopupClose;
            ModalPopupsManager.RemovePopup();
        }
    }
}