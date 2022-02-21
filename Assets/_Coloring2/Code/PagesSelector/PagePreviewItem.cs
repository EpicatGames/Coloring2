using System;
using Coloring2.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.PagesSelector
{
    [RequireComponent(typeof(Button))]
    public class PagePreviewItem : MonoBehaviour
    {
        [SerializeField] private DrawingPageConfig _config;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnTap);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnTap);
        }

        private void OnTap()
        {
            SoundsManager.PlayClick();
           Debug.Log($"config: {_config.name}");
        }
    }
}