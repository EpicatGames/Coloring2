using System;
using Cysharp.Threading.Tasks;
using PaintCraft.Canvas.Configs;
using PaintCraft.Controllers;
using UnityEngine;

namespace Coloring2.Utils
{
    [DefaultExecutionOrder(-1600)]
    [RequireComponent(typeof(Camera))]
    public class PaintCraftCameraHelper : MonoBehaviour
    {
        [SerializeField]private CanvasController _canvasController;
        [SerializeField]private float _cameraSize;
        
        private Camera _camera;

        private void Awake()
        {
            if(StartApplication.Initialized == false)
                return;
            
            _camera = GetComponent<Camera>();
            _canvasController.OnPageChange += OnPageChangeHandler;
        }

        private void OnDestroy()
        {
            if(_canvasController == null)
                return;
            _canvasController.OnPageChange += OnPageChangeHandler;
        }

        private async void OnPageChangeHandler(IPageConfig config)
        {
            await UniTask.DelayFrame(1);
            _camera.orthographicSize = _cameraSize;
            Debug.Log($"OnPageChangeHandler: {_camera.orthographicSize}");
        }
    }
}