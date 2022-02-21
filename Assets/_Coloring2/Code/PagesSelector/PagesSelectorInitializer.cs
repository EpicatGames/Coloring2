using UnityEngine;

namespace Coloring2.PagesSelector
{
    public class PagesSelectorInitializer : MonoBehaviour
    {
        private void Awake()
        {
            if (StartApplication.Initialized == false)
            {
                ScenesManager.LoadScene(ScenesManager.Scenes.StartAppScene);
            }
        }
    }
}