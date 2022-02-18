using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PaintCraft.Demo.ColoringBook
{
    public class OpenURLButtonController : MonoBehaviour
    {
        [FormerlySerializedAs("Url")] public string url;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(url));
        }
    }
}