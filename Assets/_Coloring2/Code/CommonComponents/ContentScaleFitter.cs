using UnityEngine;

namespace Coloring2.CommonComponents
{
    [RequireComponent(typeof(RectTransform))]
    public class ContentScaleFitter : MonoBehaviour
    {
        [SerializeField] 
        [Range(0, 1f)]
        private float _fitPercentageFactor;
        protected void Awake()
        {
            var screenHeight = GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta.y;
            var rect = GetComponent<RectTransform>();
            var winHeight = rect.rect.height;
            var scale = screenHeight <= winHeight ? winHeight / screenHeight : screenHeight / winHeight;
            scale *= _fitPercentageFactor;
            rect.localScale = new Vector3(scale, scale, scale);
            
            // Debug.Log($"screenHeight {screenHeight}");
            // Debug.Log($"winHeight {winHeight}");
            // Debug.Log($"scale {scale}");
        }
    }
}