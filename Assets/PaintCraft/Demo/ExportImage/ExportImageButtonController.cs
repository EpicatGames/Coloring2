using System.Diagnostics;
using System.IO;
using PaintCraft.Controllers;
using PaintCraft.Utils;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace PaintCraft.Demo.ExportImage
{
    [RequireComponent(typeof(Button))]
    public class ExportImageButtonController : MonoBehaviour
    {
        private void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(ExportImage);
        }

        private void ExportImage()
        {
            var path = Path.Combine(Application.persistentDataPath, "test.png");
            var canvasController = FindObjectOfType<CanvasController>();
            var result = canvasController.ExportResultImage();
            result.WritePngToFile(path);
            Debug.LogFormat("result image stored at {0}", path);
            Application.OpenURL(path);

#if UNITY_EDITOR
            Process.Start(path);
#endif
        }
    }
}