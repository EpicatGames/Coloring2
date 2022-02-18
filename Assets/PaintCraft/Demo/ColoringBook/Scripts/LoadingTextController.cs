using UnityEngine;
using UnityEngine.UI;

namespace PaintCraft.Demo.ColoringBook
{
    public class LoadingTextController : MonoBehaviour
    {
        private float startTime;
        private Text text;

        private void Start()
        {
            text = GetComponent<Text>();
            startTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            var dots = new string('.', (int) ((Time.realtimeSinceStartup - startTime) / 0.5) % 4);
            text.text = "Loading" + dots;
        }
    }
}