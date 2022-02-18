using PaintCraft.Canvas.Configs;
using PaintCraft.Controllers;
using PaintCraft.Tools;
using PaintCraft.UIControllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace PaintCraft.Demo
{
    public class SwitchRegionsToggleController : MonoBehaviour
    {
        [FormerlySerializedAs("NormalConfig")] public ScriptableObject normalConfig;
        [FormerlySerializedAs("RegionConfig")] public ScriptableObject regionConfig;

        [FormerlySerializedAs("NormalBrush")] public Brush normalBrush;
        [FormerlySerializedAs("RegionBrush")] public Brush regionBrush;

        [FormerlySerializedAs("BrushBtnCtrl")] public ChangeBrushOnClickController brushBtnCtrl;

        private CanvasController canvasController;

        private void Awake()
        {
            canvasController = FindObjectOfType<CanvasController>();
        }

        // Update is called once per frame
        public void UpdateCurrentPage(bool useRegionConfig)
        {
            if (useRegionConfig)
            {
                canvasController.SetActivePageConfig(regionConfig as IPageConfig);
                brushBtnCtrl.brush = regionBrush;
            }
            else
            {
                canvasController.SetActivePageConfig(normalConfig as IPageConfig);
                brushBtnCtrl.brush = normalBrush;
            }
        }
    }
}