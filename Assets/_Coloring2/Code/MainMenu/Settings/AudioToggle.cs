using Coloring2.CommonComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.MainMenu.Settings
{
    public abstract class AudioToggle : MonoBehaviour
    {
        protected ImageInfinityRotator _rotator;
        protected Toggle _toggle;
        
        protected void Awake()
        {
            _toggle = GetComponentInChildren<Toggle>();
            _rotator = GetComponentInChildren<ImageInfinityRotator>();
            _toggle.onValueChanged.AddListener(OnToggle);
        }
        
        protected void Start()
        {
            OnToggle(true);
        }
        
        protected void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(OnToggle);
        }

        protected virtual void OnToggle(bool value)
        {
            if(value)
                _rotator.Begin();
            else
                _rotator.Pause(false);
            _toggle.targetGraphic.enabled = !value;
        }
    }
}