using System;
using System.Collections.Generic;
using System.Linq;
using Coloring2.CommonComponents;
using Coloring2.Configs;
using Coloring2.DataServices;
using Coloring2.Localization;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Coloring2.MainMenu.Categories
{
    public class CategoriesInteractionsController : MonoBehaviour
    {
        public Action<CategoryConfig> CategorySelected;
        
        [SerializeField] private Swiper _swiper;

        public MenuCategory SelectedCategoryItem { get; private set; }
        
        private List<MenuCategory> _categories;
        private PlayerInteractionActionsService _playerActionsService;
        private PlayerPurchasesService _playerPurchaseService;
        private Vector2 _centerScreen;
        private Vector2 _itemSize;
        private float _itemsGap;
        
        private void OnDestroy()
        {
            if(_playerActionsService != null)
                _playerActionsService.MenuCategoryTapped -= OnMenuCategoryTapped;
            
            _swiper.onValueChanged.RemoveListener(OnScroll);
            _swiper.Swipe -= OnSwipe;
        }

        public async void Init(List<MenuCategory> categories)
        {
            _categories = categories;

            _playerPurchaseService = ServicesManager.GetService<PlayerPurchasesService>();
            _playerActionsService = ServicesManager.GetService<PlayerInteractionActionsService>();
            _playerActionsService.MenuCategoryTapped += OnMenuCategoryTapped;

            CheckIfFullVersionPurchased();
            
            var rect = _swiper.viewport.rect;
            _centerScreen = new Vector2(rect.width * .5f, rect.height * .5f);
            _swiper.SetContentPosition(new Vector2(rect.width, 0));
            _itemsGap = _swiper.content.GetComponent<HorizontalLayoutGroup>().spacing;

            var first = _categories.First();
            _itemSize = first.Size;
            await UniTask.WaitUntil(() => first.GetComponent<RectTransform>().anchoredPosition.x > 0);
            
            _swiper.onValueChanged.AddListener(OnScroll);
            _swiper.Swipe += OnSwipe;
            
            var defCategory =
                _categories.FirstOrDefault(c => c.Config.Category == Configs.Categories.category_animals);
            SelectCategoryItem(defCategory);
            UpdateItemsScaleAndOpacity();
        }

        private void CheckIfFullVersionPurchased()
        {
            var isFullversionPurchased = _playerPurchaseService.HasCategoryPurchased(Configs.Categories.full_version);
            if (!isFullversionPurchased) 
                return;
            var fullVersionItem =_categories.FirstOrDefault(c => c.Config.Category == Configs.Categories.full_version);
            _categories.Remove(fullVersionItem);
            if(fullVersionItem != null)
                fullVersionItem.gameObject.SetActive(false);
        }

        public async void ActivateAll()
        {
            for (var i = 0; i < _categories.Count; i++)
            {
                var catItem = _categories[i];
                if (catItem.Config.Category == Configs.Categories.full_version)
                {
                    _categories.Remove(catItem);
                    catItem.gameObject.SetActive(false);
                    continue;
                }
                catItem.Activate();
            }

            await UniTask.DelayFrame(5);
            var toSelect = _categories.FirstOrDefault(c => c.Config.Category == Configs.Categories.category_animals);
            SelectCategoryItem(toSelect);
        }

        private void SelectCategoryItem(MenuCategory item, bool anim = false)
        {
            var pos = _centerScreen.x - item.RectTransform.anchoredPosition.x;
            if(anim)
                _swiper.AnimateContentHorizontalTo(pos, .4f);
            else
                _swiper.SetContentPosition(new Vector2(pos, 0));
            SelectedCategoryItem = item;
        }

        private MenuCategory GetFirstCategory()
        {
           var min = float.MaxValue;
           MenuCategory result = null;
           foreach (var cat in _categories)
           {
               if (!(cat.transform.position.x < min)) 
                   continue;
               min = cat.transform.position.x;
               result = cat;
           }
           return result;
       }
       
       private MenuCategory GetLastCategory()
       {
           var max = float.MinValue;
           MenuCategory result = null;
           foreach (var cat in _categories)
           {
               if (!(cat.transform.position.x > max))
                   continue;
               max = cat.transform.position.x;
               result = cat;
           }
           return result;
       }

       private void UpdateItemsScaleAndOpacity()
        {
            foreach (var cat in _categories)cat.UpdateScaleAndOpacity();
        }

       #region Handlers
       private void OnScroll(Vector2 value)
       {
           var first = GetFirstCategory();
           var last = GetLastCategory();
           var firstRect = (RectTransform)first.transform;
           var lastRect = (RectTransform)last.transform;
            
           UpdateItemsScaleAndOpacity();
           if (first.transform.position.x >= 0)
           {
               lastRect.anchoredPosition = firstRect.anchoredPosition - new Vector2(_itemSize.x + _itemsGap, 0);
               _categories.Remove(last);
               _categories.Insert(0, last);
               return;
           }
           
           if (last.transform.position.x <= Screen.width)
           {
               firstRect.anchoredPosition = lastRect.anchoredPosition + new Vector2(_itemSize.x + _itemsGap, 0);
               _categories.Remove(first);
               _categories.Add(first);
           }
       }
        
       private void OnSwipe(Swiper.SwiperEventData data)
       {
           if (data.Delta <= Swiper.MinDeltaValueForSwipe)
           {
               _swiper.AnimateContentHorizontalTo(data.PositionOnBeginSwipe.x, .4f);
               return;
           }
           
           var index = _categories.IndexOf(SelectedCategoryItem) + (data.Direction == Swiper.Directions.Left ? 1 : -1);
           SelectCategoryItem(_categories[index], true);
           SoundsManager.PlaySwapCategory();
       }
        
       private void OnMenuCategoryTapped(MenuCategory item)
       {
           SoundsManager.PlayPickCategory();
           
           var dist = Math.Abs(Screen.width * .5f - item.transform.position.x);
           if (dist > _itemSize.x * .5f)
               SelectCategoryItem(item, true);
           else
               CategorySelected?.Invoke(item.Config);
       }
       #endregion
    }
}