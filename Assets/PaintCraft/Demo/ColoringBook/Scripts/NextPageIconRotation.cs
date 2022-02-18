using UnityEngine;
using UnityEngine.Serialization;

namespace PaintCraft.Demo.ColoringBook
{
    public class NextPageIconRotation : MonoBehaviour
    {
        [FormerlySerializedAs("ContentRect")] public RectTransform contentRect;

        [FormerlySerializedAs("MiddleYPosition")]
        public float middleYPosition = 1030.0f;

        private RectTransform selfRectTransform;

        private void Start()
        {
            selfRectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        private void Update()
        {
            var rotation = contentRect.anchoredPosition.y / middleYPosition * 180.0f;
            selfRectTransform.localRotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}