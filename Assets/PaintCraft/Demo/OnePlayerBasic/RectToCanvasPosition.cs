using PaintCraft.Controllers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class RectToCanvasPosition : MonoBehaviour
{
    [FormerlySerializedAs("_screenCameraController")] [SerializeField]
    private ScreenCameraController screenCameraController;
    private CanvasScaler cs;
    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        rt.hasChanged = true;
        Assert.IsNotNull(rt);
        cs = GetComponentInParent<CanvasScaler>();
    }

    private void LateUpdate()
    {
        if (rt.hasChanged)
        {
            rt.hasChanged = false;
            Vector2 bottomLeft, topRight;
            GetScreenRect(rt, out bottomLeft, out topRight);

            screenCameraController.cameraSize.viewPortOffset.left = (int) bottomLeft.x;
            screenCameraController.cameraSize.viewPortOffset.bottom = (int) bottomLeft.y;
            screenCameraController.cameraSize.viewPortOffset.right = Screen.width - (int) topRight.x;
            screenCameraController.cameraSize.viewPortOffset.top = Screen.height - (int) topRight.y;
            //_screenCameraController.CameraSize.Init(_screenCameraController.Camera, _screenCameraController.Canvas);
            screenCameraController.cameraSize.ResetSize();
        }
    }

    private void GetScreenRect(RectTransform rectTransform, out Vector2 bottomLeft, out Vector2 topRight)
    {
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        var canvas = rectTransform.GetComponentInParent<Canvas>();
        var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        bottomLeft = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
        topRight = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);
    }
}