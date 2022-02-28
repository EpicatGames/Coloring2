using UnityEngine;
using UnityEngine.EventSystems;

namespace Coloring2.CommonComponents
{
    public class ButtonPressScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        [Range(.5f, 1f)]
        private float _scaleFactor = .95f;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localScale = new Vector3(_scaleFactor, _scaleFactor, _scaleFactor);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}