using System;
using PaintCraft.Controllers;
using PaintCraft.UIControllers;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.PaintingPage.SystemControls
{
    public class ClearButtonController : ActivatedButton
    {
        protected override void OnTap()
        {
            SoundsManager.PlayClick();
            if (!_activated)
            {
                Activate();
                return;
            }
            _canvas.ClearCanvas();
            Deactivate();
        }
    }
}