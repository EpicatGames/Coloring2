using Coloring2.MainMenu.Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Coloring2.Configs
{
    [CreateAssetMenu(fileName = "AppConfig", menuName = "Coloring2/AppConfig", order = 0)]
    public class AppConfig : ScriptableObject
    {
        public RectTransform PopupsContainerRef;
        public ScenesManager.Scenes SceneToLoadAferStartApp;
        
        [Space(10)]
        public EnterBirthdayPopup EnterBirthdayPopupRef;
        public SettingsPopup SettingsPopupRef;
    }
}