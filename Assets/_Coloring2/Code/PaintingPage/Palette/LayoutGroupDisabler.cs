using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Coloring2.PaintingPage.Palette
{
    [RequireComponent(typeof(LayoutGroup))]
    public class LayoutGroupDisabler : MonoBehaviour
    {
        private LayoutGroup _layout;

        private async void Start()
        {
            _layout = GetComponent<LayoutGroup>();
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _layout.enabled = false;
        }
    }
}