using System;
using System.Collections.Generic;
using Coloring2.CommonComponents.PagesSwiper;
using Coloring2.Configs;
using Coloring2.DataServices;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
        private SelectedItemService _selectedItemService;
        private ScenesSwapScreen _scenesSwapScreen;
        private Color _whiteTransparent = new Color(1, 1, 1, 0);
        private TweenerCore<Color, Color, ColorOptions> _bgroundFadeTween;

        private void Awake()
        {
            _nextBground.color = _whiteTransparent;
            _backBtn.onClick.AddListener(OnBackBtn);
            _pagesSwiper.SwipeComplete += OnSwipeComplete;
        }

        private void OnDestroy()
        {
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

            await UniTask.DelayFrame(1);

            ShowSelectedPage();
            _scenesSwapScreen.FadeOut();
        }

        private void InitServices()
        {
            _selectedItemService = ServicesManager.GetService<SelectedItemService>();
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
            if (_pagesSwiper.TotalPages <= 1)
            {
                _leftSwipeBtn.gameObject.SetActive(false);
                _rightSwipeBtn.gameObject.SetActive(false);
                _pagesSwiper.Disable();
            }
            else
            {
                _leftSwipeBtn.onClick.AddListener(OnLeftSwipeBtn);
                _rightSwipeBtn.onClick.AddListener(OnRightSwipeBtn);
            }
        }
        
        private void ShowSelectedPage()
        {
            var selectedCategory = _selectedItemService.Get<CategoryConfig>();
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
            //if(_bgroundFadeTween != null && _bgroundFadeTween.IsPlaying())
                _bgroundFadeTween?.Kill();
            
            var currentPage = _pagesSwiper.GetPageByID(_pagesSwiper.CurrentPageId).transform
                .GetComponent<CategoryPageSelector>();

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
            _scenesSwapScreen.FadeInComplete += LoadMainMenuScene;
            _scenesSwapScreen.FadeIn();
        }
        
        private void LoadMainMenuScene()
        {
            _scenesSwapScreen.FadeInComplete -= LoadMainMenuScene;
            ScenesManager.LoadScene(ScenesManager.Scenes.MainMenuScene);
        }
    }
}