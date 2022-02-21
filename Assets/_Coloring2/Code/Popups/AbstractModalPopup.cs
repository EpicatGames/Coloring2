using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Coloring2.Popups
{
    public abstract class AbstractModalPopup : MonoBehaviour, IPopup
    {
        public event Action<IPopup> Closed;
        
        [SerializeField] private Image _blocker;
        [SerializeField] private RectTransform _window;
        
        private float _windowBottomYPosition;
        private EventTrigger.Entry _entry;
        [SerializeField] protected Button _closeButton;

        protected virtual void Awake()
        {
            _blocker.color = Color.clear;
            _windowBottomYPosition = -Screen.height - _window.sizeDelta.y * .7f;
            _window.localPosition = new Vector3(0, Screen.height + _window.sizeDelta.y * .7f, 0);

            var trigger = _blocker.gameObject.AddComponent<EventTrigger>();
            _entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
            _entry.callback.AddListener(OnBlockerTap);
            trigger.triggers.Add(_entry);
        }

        protected virtual void OnDestroy()
        {
            _entry.callback.RemoveListener(OnBlockerTap);
            _closeButton.onClick.RemoveListener(Close);
        }

        private void OnBlockerTap(BaseEventData data) => Close();

        public void Close()
        {
            SoundsManager.PlayClick();
            _blocker.DOFade(0, 0.4f);
            _window.DOLocalMoveY(_windowBottomYPosition, 0.4f)
                .OnComplete(() => Closed?.Invoke(this));
        }

        public virtual void Show()
        {
            _closeButton.onClick.AddListener(Close);
            _window.transform.DOLocalMoveY(0, .4f)
                .OnComplete(() => _blocker.DOFade(0.2f, 1));
        }
    }
}