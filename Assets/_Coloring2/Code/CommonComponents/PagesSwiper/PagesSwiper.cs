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
    public class PagesSwiper : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        public Action SwipeComplete;
        
        [SerializeField]private float _percentThreshold = 0.2f;
        [SerializeField]private Transform _content;
        [SerializeField]private Transform _root;
        
        public readonly List<IPage> Pages = new List<IPage>();
        public int TotalPages{ get; private set; }
        public int CurrentPageId { get; private set; } = -1;

        private readonly List<IPage> _pagesToAnimation = new List<IPage>(3);
        private readonly List<Vector2> _startPositionsAtDrag = new List<Vector2>(3);
        private RectTransform _poolContainer;
        private bool _animated;
        private bool _disabled;
        
        private void Awake()
        {
            _poolContainer = new GameObject("PoolContainer").AddComponent<RectTransform>();
            _poolContainer.SetParent(_root);
            _poolContainer.anchorMin = new Vector2(0, 0);
            _poolContainer.anchorMax = new Vector2(1, 1);
            _poolContainer.anchoredPosition = _poolContainer.sizeDelta = Vector3.zero;
            _poolContainer.gameObject.SetActive(false);
        }

        public void Disable() => _disabled = true;

        public void CreatePagesPool<T>(List<T> pagesPrefabs) where T : IPage
        {
            for (var i = 0; i < pagesPrefabs.Count; i++)
            {
                var pageRef = pagesPrefabs[i];
                var page = Instantiate(pageRef.gameObject, _poolContainer).GetComponent<IPage>();
                Pages.Add(page);
                page.Init(i);
            }
            TotalPages = _poolContainer.childCount;
        }
        
        public IPage GetPageByID(int id) => Pages.FirstOrDefault(p => p.Id == id);

        public void ShowPage(int id)
        {
            if(id >= Pages.Count || id == CurrentPageId)
                return;

            var page = ReleasePageViewFromPool(id);
            CurrentPageId = page.Id;
        }

        private IPage ReleasePageViewFromPool(int id)
        {
            var page = Pages[id];
            var rectTransform = page.RectTransform;
            rectTransform.SetParent(_content);
            return page;
        }

        private void PutPageViewBackToPool(RectTransform page)
        {
            page.SetParent(_poolContainer);
        }

        public void NextPage()
        {
            if(_animated || _disabled)
                return;
            var offset = GetPagesToSwipeNext();
            AnimateContent(-offset);
        }

        public void PrevPage()
        {
            if(_animated || _disabled)
                return;
            var offset = GetPagesToSwipePrev();
            AnimateContent(offset);
        }
        
        private float GetPagesToSwipeNext()
        {
            //_pagesToAnimation.Clear();
            var currentPage = Pages[CurrentPageId];
            var nextID = CurrentPageId + 1 >= Pages.Count ? 0 : CurrentPageId + 1;
            var nextPage = ReleasePageViewFromPool(nextID);
            var offset = nextPage.GetSize().x;
            nextPage.RectTransform.anchoredPosition = currentPage.RectTransform.anchoredPosition + new Vector2(offset, 0);
            
            _pagesToAnimation.Add(currentPage);
            _pagesToAnimation.Add(nextPage);
            CurrentPageId = nextID;
            return offset;
        }
        
        private float GetPagesToSwipePrev()
        {
            //_pagesToAnimation.Clear();
            var currentPage = Pages[CurrentPageId];
            var prevID = CurrentPageId - 1 <= 0 ? Pages.Count - 1 : CurrentPageId - 1;
            var prevPage = ReleasePageViewFromPool(prevID);

            var offset = prevPage.GetSize().x;
            prevPage.RectTransform.anchoredPosition = currentPage.RectTransform.anchoredPosition + new Vector2(-offset, 0);
            
            _pagesToAnimation.Add(prevPage);
            _pagesToAnimation.Add(currentPage);
            CurrentPageId = prevID;
            return offset;
        }
        
        private void GetPagesToDrag()
        {
            //_pagesToAnimation.Clear();
            var currentPage = Pages[CurrentPageId];
            
            var prevID = CurrentPageId - 1 <= 0 ? Pages.Count - 1 : CurrentPageId - 1;
            var nextID = CurrentPageId + 1 >= Pages.Count ? 0 : CurrentPageId + 1;
            
            var prevPage = ReleasePageViewFromPool(prevID);
            var nextPage = ReleasePageViewFromPool(nextID);

            var offset = prevPage.GetSize().x;
            var currPagePosition = currentPage.RectTransform.anchoredPosition;
            prevPage.RectTransform.anchoredPosition = currPagePosition + new Vector2(-offset, 0);
            nextPage.RectTransform.anchoredPosition = currPagePosition + new Vector2(offset, 0);
            
            _pagesToAnimation.Add(prevPage);
            _pagesToAnimation.Add(currentPage);
            _pagesToAnimation.Add(nextPage);
            _startPositionsAtDrag.Add(prevPage.RectTransform.anchoredPosition);
            _startPositionsAtDrag.Add(currentPage.RectTransform.anchoredPosition);
            _startPositionsAtDrag.Add(nextPage.RectTransform.anchoredPosition);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_animated || _disabled)
                return;
            GetPagesToDrag();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if(_animated || _disabled)
                return;

            var delta = eventData.pressPosition.x - eventData.position.x;
            for (var i = 0; i < _pagesToAnimation.Count; i++)
            {
                var p = _pagesToAnimation[i];
                var pos = _startPositionsAtDrag[i];
                p.RectTransform.anchoredPosition = pos - new Vector2(delta, 0);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(_animated || _disabled || _pagesToAnimation.Count <= 0)
                return;
            
            var pageSize = Pages[CurrentPageId].GetSize();
            var delta = eventData.pressPosition.x - eventData.position.x;
            var cursorOffset = delta / pageSize.x;
            float offset;
            if(Mathf.Abs(cursorOffset) >= _percentThreshold)
            {
                var page = delta < 0 ? _pagesToAnimation[0] : _pagesToAnimation[2];
                offset = page.RectTransform.anchoredPosition.x;
                CurrentPageId = page.Id;
                AnimateContent(-offset);
                SoundsManager.PlaySwapCategory();
            }
            else
            {
                offset = _pagesToAnimation.First().RectTransform.anchoredPosition.x - _startPositionsAtDrag.First().x;
                AnimateContent(-offset, true);
            }
        }
        
        private void AnimateContent(float offset, bool isSamePage = false)
        {
            if(_animated)
                return;

            _animated = true;
            foreach (var p in _pagesToAnimation)
            {
                var xpos = (p.RectTransform.anchoredPosition + new Vector2(offset, 0)).x;
                p.RectTransform.DOAnchorPosX(xpos, .4f)
                    .OnComplete(() =>
                    {
                        _animated = false;
                        if(_pagesToAnimation.Count == 2)
                            PutPageViewBackToPool(offset < 0 ? _pagesToAnimation[0].RectTransform : _pagesToAnimation[1].RectTransform);
                        else
                        {
                            foreach (var p in _pagesToAnimation)
                            {
                                if (Math.Abs(p.RectTransform.anchoredPosition.x) > 100f)
                                    PutPageViewBackToPool(p.RectTransform);
                            }
                        }
                        _pagesToAnimation.Clear();
                        _startPositionsAtDrag.Clear();
                    
                        if(!isSamePage)
                            SwipeComplete?.Invoke();
                    });
            }
        }
    }
}