using System;
using System.Collections.Generic;
using Coloring2.CommonComponents.PagesSwiper;
using Coloring2.Configs;
using Coloring2.DataServices;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using PaintCraft.Canvas;
using PaintCraft.Canvas.Configs;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Coloring2.PagesSelector
{
    public class PagesSelectorController : MonoBehaviour
    {
        [SerializeField] private PagesSwiper _pagesSwiper;
        [SerializeField] private List<CategoryPageSelector> _pagesRefs;
        [SerializeField] private Button _leftSwipeBtn;
        [SerializeField] private Button _rightSwipeBtn;
        [SerializeField] private Button _backBtn;
        
        [Space(5)]
        [SerializeField] private Image _currentBground;
        [SerializeField] private Image _nextBground;

        private PlayerPurchasesService _purchaseService;
        private SelectedCategoryService _selectedCategoryService;
        private PlayerInteractionActionsService _playerInteractionsService;
        
        private ScenesSwapScreen _scenesSwapScreen;
        private Color _whiteTransparent = new Color(1, 1, 1, 0);
        private TweenerCore<Color, Color, ColorOptions> _bgroundFadeTween;

        private void Awake()
        {
            if (StartApplication.Initialized == false)
            {
                ScenesManager.LoadScene(ScenesManager.Scenes.StartAppScene);
                return;
            }
            _nextBground.color = _whiteTransparent;
        }

        private void OnDestroy()
        {
            if(_playerInteractionsService != null)
                _playerInteractionsService.PagePreviewSelected -= OnPagePreviewSelected;
            _pagesSwiper.SwipeComplete -= OnSwipeComplete;
            _leftSwipeBtn.onClick.RemoveListener(OnLeftSwipeBtn);
            _rightSwipeBtn.onClick.RemoveListener(OnRightSwipeBtn);
            _backBtn.onClick.RemoveListener(OnBackBtn);
        }

        private async void Start()
        {
            InitServices();
            AddPurchasedPages();
            InitControls();
            AddListeners();

            await UniTask.DelayFrame(1);

            ShowSelectedPage();
            _scenesSwapScreen.FadeOut();
        }

        private void InitServices()
        {
            _playerInteractionsService = ServicesManager.GetService<PlayerInteractionActionsService>();
            _selectedCategoryService = ServicesManager.GetService<SelectedCategoryService>();
            _purchaseService = ServicesManager.GetService<PlayerPurchasesService>();
            _scenesSwapScreen = ServicesManager.GetService<ProjectContextService>().ScenesSwapScreen;
        }
        
        private void AddPurchasedPages()
        {
            var pages = new List<IPage>();
            foreach (var cat in _purchaseService.PurchasedCategories)
            {
                foreach (var pageRef in _pagesRefs)
                {
                    if(cat == pageRef.Config.Category)
                        pages.Add(pageRef.GetComponent<IPage>());
                }
            }
            _pagesSwiper.AddPagesPrefabs(pages);
        }
        
        private void InitControls()
        {
            if (_pagesSwiper.TotalPages > 1) 
                return;
            _leftSwipeBtn.gameObject.SetActive(false);
            _rightSwipeBtn.gameObject.SetActive(false);
            _pagesSwiper.Disable();
        }
        
        private void AddListeners()
        {
            _playerInteractionsService.PagePreviewSelected += OnPagePreviewSelected;
            _leftSwipeBtn.onClick.AddListener(OnLeftSwipeBtn);
            _rightSwipeBtn.onClick.AddListener(OnRightSwipeBtn);
            _backBtn.onClick.AddListener(OnBackBtn);
            _pagesSwiper.SwipeComplete += OnSwipeComplete;
        }

        private void OnPagePreviewSelected(PageConfigSO config)
        {
            AppData.SelectedPageConfig = config;
            _scenesSwapScreen.FadeIn(() => ScenesManager.LoadScene(ScenesManager.Scenes.PaintingScene));
        }

        private void ShowSelectedPage()
        {
            var selectedCategory = _selectedCategoryService.Get<CategoryConfig>();
            _pagesSwiper.ShowPage(GetPageIdByCategory(selectedCategory));
            _currentBground.sprite = selectedCategory.PageBground;
        }

        private int GetPageIdByCategory(CategoryConfig selectedCategory)
        {
            foreach (var p in _pagesSwiper.Pages)
            {
                var pSelector = p.transform.GetComponent<CategoryPageSelector>();
                if (pSelector.Config == selectedCategory)
                    return p.Id;
            }
            return 0;
        }

        private void OnLeftSwipeBtn()
        {
            SoundsManager.PlayClick();
            _pagesSwiper.PrevPage();
        }

        private void OnRightSwipeBtn()
        {
            SoundsManager.PlayClick();
            _pagesSwiper.NextPage();
        }

        private void OnSwipeComplete()
        {
            _bgroundFadeTween?.Kill();
            
            var currentPage = _pagesSwiper.GetPageByID(_pagesSwiper.CurrentPageId).transform
                .GetComponent<CategoryPageSelector>();
            _selectedCategoryService.Set(currentPage.Config);

            var bgToShow = currentPage.Config.PageBground;
            if(_nextBground.color != _whiteTransparent)
                _nextBground.color = _whiteTransparent;
            _nextBground.sprite = bgToShow;

            _bgroundFadeTween = _nextBground.DOFade(1, 1f)
                .OnComplete(() =>
                {
                    _currentBground.sprite = bgToShow;
                    _nextBground.color = _whiteTransparent;
                    _nextBground.sprite = null;
                });
        }
        
        private void OnBackBtn()
        {
            SoundsManager.PlayClick();
            _scenesSwapScreen.FadeIn(() => ScenesManager.LoadScene(ScenesManager.Scenes.MainMenuScene));
        }
    }
}