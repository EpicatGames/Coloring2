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
        public Action<CategoryConfig> TryPurchaseCategory;
        
        [SerializeField] private Swiper _swiper;

        private List<MenuCategory> _categories;

        private PlayerInteractionActionsService _playerActionsService;
        private PlayerPurchasesService _playerPurchasesService;

        private RectTransform _viewport;
        private RectTransform _contentRect;
        private Vector2 _centerScreen;
        private Vector2 _itemSize;
        private float _itemsGap;
        private MenuCategory _selectedCategory;

        public async void Init(List<MenuCategory> categories)
        {
            _categories = categories;
            
            _playerPurchasesService = ServicesManager.GetService<PlayerPurchasesService>();
            _playerActionsService = ServicesManager.GetService<PlayerInteractionActionsService>();
            _playerActionsService.MenuCategoryTapped += OnMenuCategoryTapped;

            _viewport = _swiper.viewport;
            _centerScreen = new Vector2(_viewport.rect.width * .5f, _viewport.rect.height * .5f);
            _contentRect = _swiper.content.GetComponent<RectTransform>();
            _contentRect.anchoredPosition = new Vector2(_viewport.rect.width, 0);
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

        private void OnDestroy()
        {
            if(_playerActionsService != null)
                _playerActionsService.MenuCategoryTapped -= OnMenuCategoryTapped;
            
            _swiper.onValueChanged.RemoveListener(OnScroll);
            _swiper.Swipe -= OnSwipe;
        }

        private void SelectCategoryItem(MenuCategory item, bool anim = false)
        {
            var pos = _centerScreen.x - item.RectTransform.anchoredPosition.x;
            if(anim)
                _contentRect.DOAnchorPosX(pos, .4f);
            else
                _contentRect.anchoredPosition = new Vector2(pos, 0);
            _selectedCategory = item;
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
           //Debug.Log($"first: {first}, x: {first.transform.position.x}");
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
               _contentRect.DOAnchorPosX(data.PositionOnBeginSwipe.x, .4f);
               return;
           }
           
           var index = _categories.IndexOf(_selectedCategory) + (data.Direction == Swiper.Directions.Left ? 1 : -1);
           //Debug.Log($"{string.Join(",", _categories.Select(c => c.gameObject.name))}");
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
           {
               if (item.Config.PurchasedByDefault || _playerPurchasesService.HasCategoryPurchased(item.Config.Category))
               {
                   return;
               }
               TryPurchaseCategory?.Invoke(item.Config);
           }
       }
       #endregion
    }
}