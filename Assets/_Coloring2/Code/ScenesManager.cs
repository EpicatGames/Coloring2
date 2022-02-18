using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Coloring2
{
    public class ScenesManager
    {
        public static Action SceneRemoved;
        
        public enum Scenes
        {
            StartAppScene,
            SpashScreenScene,
            MainMenuScene,
            SelectPageScene,
            PaintingScene
        }
        
        public static Scenes Current { get; private set; }

        public static void LoadScene(Scenes scene, Action callback)
        {
            Current = scene;
            var operation = SceneManager.LoadSceneAsync(scene.ToString());
            UniTask.WaitUntil(() => operation.isDone)
                .ContinueWith(() =>
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int) scene));
                    callback?.Invoke();
                });
        }
        
        public static void LoadScene(Scenes scene)
        {
            Current = scene;
            var operation = SceneManager.LoadSceneAsync(scene.ToString());
            UniTask.WaitUntil(() => operation.isDone)
                .ContinueWith(() => SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int) scene)));
        }

        public static void ChangeScene(Scenes oldScene, Scenes newScene)
        {
            if(SceneManager.sceneCountInBuildSettings <= (int)newScene)
                return;

            var operation = SceneManager.UnloadSceneAsync((int)oldScene);
            UniTask.WaitUntil(() => operation.isDone)
                .ContinueWith(() =>
                {
                    SceneRemoved?.Invoke();
                    Current = newScene;
                    var op = SceneManager.LoadSceneAsync((int)newScene, LoadSceneMode.Additive);
                    UniTask.WaitUntil(() => op.isDone)
                        .ContinueWith(() =>
                        {
                            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int) newScene));
                        });
                });
        }

        public static void ChangeScene(Scenes oldScene, Scenes newScene, Action callback)
        {
            if(SceneManager.sceneCountInBuildSettings <= (int)newScene)
                return;

            var operation = SceneManager.UnloadSceneAsync((int)oldScene);
            UniTask.WaitUntil(() => operation.isDone)
                .ContinueWith(() =>
                {
                    SceneRemoved?.Invoke();
                    var op = SceneManager.LoadSceneAsync((int)newScene, LoadSceneMode.Additive);
                    Current = newScene;
                    UniTask.WaitUntil(() => op.isDone)
                        .ContinueWith(() =>
                        {
                            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int) newScene));
                            callback.Invoke();
                        });
                });
        }
    }
}