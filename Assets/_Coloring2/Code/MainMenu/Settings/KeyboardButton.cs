using System;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.MainMenu.Settings
{
    [RequireComponent(typeof(Button))]
    public class KeyboardButton : MonoBehaviour
    {
        private event Action<int> _tapCallback; 
        
        [SerializeField] private int _value;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnTap);
        }

        public void Init(Action<int> tapCallback)
        {
            _tapCallback = tapCallback;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnTap);
        }

        private void OnTap()
        {
            _tapCallback?.Invoke(_value);
        }
    }
}