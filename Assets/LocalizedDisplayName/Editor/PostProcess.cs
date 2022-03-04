using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class PostProcess: MonoBehaviour
{
    [PostProcessBuildAttribute(9999)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS) {
            OnIOSBuild(target, path);
        }
    }

    private static void OnIOSBuild(BuildTarget target, string path) {
        LDNWindow.AddLocalizedStringsIOS(path, Path.Combine(Application.dataPath, "LDN", "iOS"));
    }
}
