using Coloring2.MainMenu.Settings;
using Coloring2.PaintingPage;
using Coloring2.Popups;
using UnityEngine;
using UnityEngine.Serialization;

namespace Coloring2.Configs
{
    [CreateAssetMenu(fileName = "AppConfig", menuName = "Coloring2/AppConfig", order = 0)]
    public class AppConfig : ScriptableObject
    {
#if UNITY_EDITOR
        public enum ScenesToLoad
        {
            SpashScreenScene = 1,
            MainMenuScene = 2,
            SelectPageScene = 3,
            PaintingScene = 4
        }
        public ScenesToLoad SceneToLoadAferStartApp;
#endif
        
        [Space(10)]
        public RectTransform PopupsContainerRef;
        public ScenesSwapScreen ScenesSwapScreenRef;
        
        [Space(10)]
        public EnterBirthdayPopup EnterBirthdayPopupRef;
        public SettingsPopup SettingsPopupRef;
        public ColorsPurchasePopup ColorsPurchasePopupRef;
    }
}