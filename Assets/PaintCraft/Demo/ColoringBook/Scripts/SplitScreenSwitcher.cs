using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PaintCraft.Demo.ColoringBook
{
    public class SplitScreenSwitcher : MonoBehaviour
    {
        [FormerlySerializedAs("SingleViewObjects")]
        public List<GameObject> singleViewObjects;
        [FormerlySerializedAs("SplitScreenObjects")]
        public List<GameObject> splitScreenObjects;

        [FormerlySerializedAs("ShowSplitScreenByDefault")]
        public bool showSplitScreenByDefault;

        private void Start()
        {
            ShowSpleetScreen(showSplitScreenByDefault);
        }

        public void ShowSpleetScreen(bool showSplitScreen)
        {
            splitScreenObjects.ForEach(obj => obj.SetActive(showSplitScreen));
            singleViewObjects.ForEach(obj => obj.SetActive(!showSplitScreen));
        }
    }
}