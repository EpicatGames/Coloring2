using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Canvas))]
public class CanvasHelper : MonoBehaviour
{
    private static List<CanvasHelper> Helpers = new List<CanvasHelper>();
    public static readonly UnityEvent OnResolutionOrOrientationChanged = new UnityEvent();
    private static bool ScreenChangeVarsInitialized;
    private static ScreenOrientation LastOrientation = ScreenOrientation.Landscape;
    private static Vector2 LastResolution = Vector2.zero;
    private static Rect LastSafeArea = Rect.zero;
 
    private Canvas canvas;
    [SerializeField] private RectTransform SafeAreaTransform;

    private void Awake()
    {
        if(!Helpers.Contains(this))
            Helpers.Add(this);
   
        canvas = GetComponent<Canvas>();

        if(!ScreenChangeVarsInitialized)
        {
            LastOrientation = Screen.orientation;
            LastResolution.x = Screen.width;
            LastResolution.y = Screen.height;
            LastSafeArea = Screen.safeArea;
   
            ScreenChangeVarsInitialized = true;
        }
        ApplySafeArea();
    }

    private void Update()
    {
        if(Helpers[0] != this)
            return;
   
        if(Application.isMobilePlatform && Screen.orientation != LastOrientation)
            OrientationChanged();
   
        if(Screen.safeArea != LastSafeArea)
            SafeAreaChanged();
   
        if(Screen.width != LastResolution.x || Screen.height != LastResolution.y)
            ResolutionChanged();
    }

    private void ApplySafeArea()
    {
        if(SafeAreaTransform == null)
            return;
   
        var safeArea = Screen.safeArea;
   
        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;
   
        SafeAreaTransform.anchorMin = anchorMin;
        SafeAreaTransform.anchorMax = anchorMax;
    }

    private void OnDestroy()
    {
        if(Helpers != null && Helpers.Contains(this))
            Helpers.Remove(this);
    }
 
    private static void OrientationChanged()
    {
        //Debug.Log("Orientation changed from " + lastOrientation + " to " + Screen.orientation + " at " + Time.time);
   
        LastOrientation = Screen.orientation;
        LastResolution.x = Screen.width;
        LastResolution.y = Screen.height;
 
        OnResolutionOrOrientationChanged.Invoke();
    }
 
    private static void ResolutionChanged()
    {
        //Debug.Log("Resolution changed from " + lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);
   
        LastResolution.x = Screen.width;
        LastResolution.y = Screen.height;
 
        OnResolutionOrOrientationChanged.Invoke();
    }
 
    private static void SafeAreaChanged()
    {
        // Debug.Log("Safe Area changed from " + lastSafeArea + " to " + Screen.safeArea.size + " at " + Time.time);
        LastSafeArea = Screen.safeArea;
   
        for (int i = 0; i < Helpers.Count; i++)
        {
            Helpers[i].ApplySafeArea();
        }
    }
}