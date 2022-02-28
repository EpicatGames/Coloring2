using System;
using Coloring2.PaintingPage.Palette;
using DG.Tweening;
using PaintCraft.Tools;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Coloring2.PaintingPage.Tools
{
    public abstract class AbstractPaintTool : MonoBehaviour, IPaintTool
    {
        [SerializeField] protected ToolTypes _type;
        [SerializeField] protected Brush _brush;

        [Space(5)]
        [SerializeField] protected Button _button;

        [SerializeField] protected Transform _movablePart;

        protected Action<IPaintTool> _tapCallback;
        protected LineConfig _lineConfig;
        protected IPalette _palette;

        public ToolTypes Type => _type;
        public Brush Brush { get; }

        protected virtual void Awake()
        {
            _button.onClick.AddListener(OnTap);
        }
        
        protected virtual void OnDestroy()
        {
            _button.onClick.RemoveListener(OnTap);
        }

        public void Init(Action<IPaintTool> tapCallback, IPalette palette, LineConfig lineConfig)
        {
            _tapCallback = tapCallback;
            _palette = palette;
            _lineConfig = lineConfig;
        }

        public abstract void UpdateState(IPaletteCell cell);

        public virtual void Select()
        {
            _lineConfig.brush = _brush;
            _movablePart.DOLocalMoveX(-45, .2f);
        }

        public virtual void Deselect()
        {
            _movablePart.DOLocalMoveX(0, .2f);
        }

        protected virtual void OnTap()
        {
            SoundsManager.PlayClick();
            _tapCallback?.Invoke(this);
        }
        
        public override string ToString()
        {
            return $"[Tool: {gameObject.name}]";
        }
    }
}