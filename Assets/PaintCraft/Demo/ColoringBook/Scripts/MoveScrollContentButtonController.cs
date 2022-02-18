using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PaintCraft.Demo.ColoringBook
{
    public class MoveScrollContentButtonController : MonoBehaviour
    {
        [FormerlySerializedAs("ScrollRect")] public ScrollRect scrollRect;
        [FormerlySerializedAs("VelocityPerClick")]
        public Vector2 velocityPerClick;
        [FormerlySerializedAs("LockOnPosition")]
        public Vector2 lockOnPosition;
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void Update()
        {
            button.interactable = scrollRect.normalizedPosition != lockOnPosition;
        }

        private void OnButtonClicked()
        {
            scrollRect.normalizedPosition = new Vector2(
                velocityPerClick.x != 0.0f ? Mathf.Clamp(scrollRect.normalizedPosition.x, 0.001f, 0.999f) : 0.0f,
                velocityPerClick.y != 0.0f ? Mathf.Clamp(scrollRect.normalizedPosition.y, 0.001f, 0.999f) : 0.0f);
            scrollRect.velocity += velocityPerClick;
        }
    }
}