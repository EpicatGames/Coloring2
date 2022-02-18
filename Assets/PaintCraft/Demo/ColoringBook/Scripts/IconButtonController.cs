using System.Collections;
using PaintCraft.Canvas;
using PaintCraft.Canvas.Configs;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PaintCraft.Demo.ColoringBook
{
    public class IconButtonController : MonoBehaviour
    {
        [SerializeField] public ScriptableObject pageScriptableObject;
        private IPageConfig pageConfig;

        private IEnumerator Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
            pageConfig = pageScriptableObject as IPageConfig;
            Assert.IsNotNull(pageConfig);

            if (Application.platform == RuntimePlatform.WebGLPlayer && pageConfig is IDownloadIconAsync)
            {

                yield return StartCoroutine((pageConfig as IDownloadIconAsync)?.DownloadIcon());
            }

            if (pageConfig.HasUserOrDefaultIcon(out var icon))
                button.targetGraphic.GetComponent<RawImage>().texture = icon;
        }

        private void OnButtonClick()
        {
            AppData.SelectedPageConfig = pageConfig;
            SceneManager.LoadScene("ColoringBook");
        }
    }
}