using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Coloring2.CommonComponents.PagesSwiper
{
    public class PagesSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public Action SwipeComplete;
        
        [SerializeField]private float _percentThreshold = 0.2f;
        [SerializeField]private Transform _content;
        [SerializeField]private bool IsCarousel;
        

        public readonly List<IPage> Pages = new List<IPage>();
        public int TotalPages{ get; private set; }
        public int CurrentPageId { get; private set; }

        private Vector2 _panelLocation;
        private int _currentPage = 1;

        private RectTransform _contentRectTransform;
        
        private bool _animated;
        private bool _disabled;

        private void Start()
        {
            _contentRectTransform = (RectTransform) _content;
            _panelLocation = _contentRectTransform.anchoredPosition;
        }

        public void Disable() => _disabled = true;

        public IPage GetPageByID(int id) => Pages.FirstOrDefault(p => p.Id == id);

        public void AddPages<T>(List<T> pages) where T : IPage
        {
            for (var i = 0; i < pages.Count; i++)
            {
                var page = pages[i];
                page.transform.SetParent(_content);
                Pages.Add(page);
                page.Init(i);
            }
            TotalPages = _content.childCount;
        }

        public void AddPage<T>(IPage page) where T : IPage
        {
            page.transform.SetParent(_content);
            page.transform.SetAsLastSibling();
            page.Init(page.transform.GetSiblingIndex());
            Pages.Add(page);
            TotalPages = _content.childCount;
        }
        
        public void AddPagesPrefabs<T>(List<T> pagesPrefabs) where T : IPage
        {
            for (var i = 0; i < pagesPrefabs.Count; i++)
            {
                var pageRef = pagesPrefabs[i];
                var page = Instantiate(pageRef.transform.gameObject, _content).GetComponent<IPage>();
                Pages.Add(page);
                page.Init(i);
            }
            TotalPages = _content.childCount;
        }
        
        public void AddPagePrefab<T>(IPage pagePrefab) where T : IPage
        {
            var page = Instantiate(pagePrefab.transform.gameObject, _content).GetComponent<IPage>();
            page.transform.SetAsLastSibling();
            page.Init(page.transform.GetSiblingIndex());
            TotalPages = _content.childCount;
        }

        public void ShowPage(int id)
        {
            if(id >= Pages.Count || id == CurrentPageId)
                return;
            
            var newX = Pages[id].Size.x * id;
            _contentRectTransform.anchoredPosition = new Vector2(-newX, _contentRectTransform.anchoredPosition.y);
            _panelLocation = _contentRectTransform.anchoredPosition;
            CurrentPageId = id;
        }

        public void AnimateToPage(int id)
        {
            if(id >= Pages.Count || id == CurrentPageId)
                return;
            
            var newX = Pages[id].Size.x * id;
            AnimateContent(-newX);
            CurrentPageId = id;
        }

        public void NextPage()
        {
            if(_animated || _disabled)
                return;
            
            var currentPage = Pages[CurrentPageId];
            var offset = currentPage.Size.x;
            int nextID;
            
            if (IsCarousel == false)
            {
                nextID = CurrentPageId + 1;
                if(nextID > Pages.Count - 1)
                    return;
            }
            else
            {
                if (GetLastPage() == currentPage)
                {
                    var firstPage = GetFirstPage();
                    nextID = firstPage.Id;
                    var firstTransform = (RectTransform)firstPage.transform;
                    var currentTransform = (RectTransform)currentPage.transform;
                    firstTransform.anchoredPosition = currentTransform.anchoredPosition + new Vector2(offset, 0);
                }
                else
                {
                    var ind = currentPage.Id < TotalPages - 1 ? currentPage.Id + 1 : 0;
                    nextID = Pages[ind].Id;
                }
            }
            
            var newLocation = _contentRectTransform.anchoredPosition + new Vector2(-offset, 0);
            AnimateContent(newLocation.x);
            CurrentPageId = nextID;
        }
        
        public void PrevPage()
        {
            if(_animated || _disabled)
                return;
            var currentPage = Pages[CurrentPageId];
            var offset = currentPage.Size.x;
            int prevID;
            
            if (IsCarousel == false)
            {
                prevID = CurrentPageId - 1;
                if(prevID < 0)
                    return;
            }
            else
            {
                if (GetFirstPage() == currentPage)
                {
                    var lastPage = GetLastPage();
                    prevID = lastPage.Id;
                    var lastTransform = (RectTransform)lastPage.transform;
                    var currentTransform = (RectTransform)currentPage.transform;
                    lastTransform.anchoredPosition = currentTransform.anchoredPosition - new Vector2(offset, 0);
                    //Debug.Log($"screen width: {Screen.width}, page width {firstPage.Size.x}, rectX: {firstTransform.rect.x}");
                }
                else
                {
                    var ind = currentPage.Id > 0 ? currentPage.Id - 1 : TotalPages - 1;
                    prevID = Pages[ind].Id;
                }
            }
            
            var newLocation = _contentRectTransform.anchoredPosition + new Vector2(offset, 0);
            AnimateContent(newLocation.x);
            CurrentPageId = prevID;
        }

        public void OnDrag(PointerEventData data)
        {
            if(!IsCarousel || _animated || _disabled) 
                return;
            
            var first = GetFirstPage();
            var last = GetLastPage();
            var firstTransform = (RectTransform)first.transform;
            var lastTransform = (RectTransform)last.transform;
            
            if (GetRectLeftTopWorldCoord(firstTransform) > 0)
            {
                lastTransform.anchoredPosition = firstTransform.anchoredPosition - new Vector2(last.Size.x, 0);
                return;
            }
            //Debug.Log($"last {last}, pos: {GetRectLeftTopWorldCoord(lastTransform)}, rectX: {lastTransform.rect.x}");
            if (GetRectLeftTopWorldCoord(lastTransform) < Math.Abs(lastTransform.rect.x * .5f))
                firstTransform.anchoredPosition = lastTransform.anchoredPosition + new Vector2(last.Size.x, 0);

            var delta = data.pressPosition.x - data.position.x;
            _contentRectTransform.anchoredPosition = _panelLocation - new Vector2(delta, 0);
        }

        public void OnEndDrag(PointerEventData data)
        {
            if(_animated || _disabled)
                return;
            
            var pageSize = Pages[CurrentPageId].Size;
            var delta = data.pressPosition.x - data.position.x;
            var cursorOffset = delta / pageSize.x;
            if(Mathf.Abs(cursorOffset) >= _percentThreshold)
            {
                var newLocation = IsCarousel
                    ? HandleSwipeInCarouselMode(_panelLocation, delta, pageSize) 
                    : HandleSwipeInNormalMode(_panelLocation, cursorOffset, pageSize);
                AnimateContent(newLocation.x);
                SoundsManager.PlaySwapCategory();
            }
            else
                AnimateContent(_panelLocation.x, true);
        }

        private Vector2 HandleSwipeInCarouselMode(Vector2 newLocation, float delta, Vector2 pageSize)
        {
            if (delta < 0)
            {
                newLocation += new Vector2(pageSize.x, 0);
                CurrentPageId--;
                if (CurrentPageId < 0) CurrentPageId = TotalPages - 1;
            }
            else
            {
                newLocation += new Vector2(-pageSize.x, 0);
                CurrentPageId++;
                if (CurrentPageId > TotalPages - 1) CurrentPageId = 0;
            }
            //Debug.Log($"CurrentPageId: {CurrentPageId}");
            _currentPage = CurrentPageId + 1;
            return newLocation;
        }
        
        private Vector2 HandleSwipeInNormalMode(Vector2 newLocation, float cursorOffset, Vector2 pageSize)
        {
            if(cursorOffset > 0 && _currentPage < TotalPages)
            {
                _currentPage++;
                newLocation += new Vector2(-pageSize.x, 0);
            }
            else if(cursorOffset < 0 && _currentPage > 1)
            {
                _currentPage--;
                newLocation += new Vector2(pageSize.x, 0);
            }
            CurrentPageId = _currentPage - 1;
            return newLocation;
        }
        
        private async void AnimateContent(float xpos, bool samePage = false)
        {
            if(_animated)
                return;

            _animated = true;
            _contentRectTransform.DOAnchorPosX(xpos, .4f)
                .OnComplete(() =>
                {
                    _animated = false;
                    _panelLocation = _contentRectTransform.anchoredPosition;
                });

            if(samePage)
                return;
            
            await UniTask.Delay(TimeSpan.FromSeconds(.2f));
            SwipeComplete?.Invoke();
        }
        
        private float GetRectLeftTopWorldCoord(RectTransform rect)
        {
            var a = new Vector3[4];
            rect.GetWorldCorners(a);
            return a.First().x;
        }
        
        private IPage GetFirstPage()
        {
            var min = float.MaxValue;
            IPage result = null;
            foreach (var page in Pages)
            {
                if (!(page.transform.position.x < min)) 
                    continue;
                min = page.transform.position.x;
                result = page;
            }
            return result;
        }
       
        private IPage GetLastPage()
        {
            var max = float.MinValue;
            IPage result = null;
            foreach (var page in Pages)
            {
                if (!(page.transform.position.x > max))
                    continue;
                max = page.transform.position.x;
                result = page;
            }
            return result;
        }
    }
}