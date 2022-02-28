using System;
using PaintCraft.Controllers;
using PaintCraft.UIControllers;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.PaintingPage.SystemControls
{
    public class UndoButtonController : ActivatedButton
    {
        private void Update()
        {
            if (!_canvas.HasUndo && _activated)
                Deactivate();
        }

        protected override void OnTap()
        {
            SoundsManager.PlayClick();
            if(!_canvas.HasUndo)
                return;

            if (!_activated)
            {
                Activate();
                return;
            }
            _canvas.Undo();
        }
    }
}