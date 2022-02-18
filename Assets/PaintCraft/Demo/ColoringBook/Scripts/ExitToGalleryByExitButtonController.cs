using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaintCraft.Demo.ColoringBook
{
    public class ExitToGalleryByExitButtonController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                SceneManager.LoadScene("PageSelect");
            }
        }
    }
}