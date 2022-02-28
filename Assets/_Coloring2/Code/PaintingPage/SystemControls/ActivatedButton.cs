using PaintCraft.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.PaintingPage.SystemControls
{
    public abstract class ActivatedButton : MonoBehaviour
    {
        [SerializeField] protected CanvasController _canvas;
        
        protected Button _button;
        protected bool _activated;

        protected void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnTap);
            Deactivate();
        }

        protected void Activate()
        {
            _button.targetGraphic.color = Color.white;
            _activated = true;
        }
        
        protected void Deactivate()
        {
            _button.targetGraphic.color = new Color(1, 1, 1, .4f);
            _activated = false;
        }

        protected abstract void OnTap();
    }
}