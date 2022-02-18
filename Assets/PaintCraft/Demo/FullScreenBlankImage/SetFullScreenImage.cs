using PaintCraft.Canvas.Configs;
using PaintCraft.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Paintcraft.Demo
{
    public class SetFullScreenImage : MonoBehaviour
    {
        [SerializeField] private string imagePath = "FullScreenImage";
        [SerializeField] private int version = 0;
        
        [FormerlySerializedAs("_canvasController")] [SerializeField]
        private CanvasController canvasController;

        private void Awake()
        {
            PagePath DefaultPath = new PagePath(imagePath, version);
            var blankPageConfig = new BlankPageConfig(DefaultPath, Screen.width, Screen.height);
            canvasController.SetActivePageConfig(blankPageConfig);
        }
    }
}