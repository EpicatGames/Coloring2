using System;
using Coloring2.DataServices;
using UnityEngine;

namespace Coloring2.MainMenu
{
    public class MainMenuSceneInitializer : MonoBehaviour
    {
        private void Awake()
        {
            if (StartApplication.Initialized == false)
            {
                ScenesManager.LoadScene(ScenesManager.Scenes.StartAppScene);
                return;
            }
            
            ServicesManager.Register<PlayerInteractionActionsService>(new PlayerInteractionActionsService());
            if(SoundsManager.MusicOn)
                SoundsManager.PlayBackgroundMusic();
        }
    }
}