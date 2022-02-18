using System;
using Coloring2.Configs;
using Coloring2.Localization;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace Coloring2
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class SplashAnimationController : MonoBehaviour
    {
        private AudioConfig _soundsConfig;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            if(AppInitializer.Initialized == false)
                ScenesManager.LoadScene(ScenesManager.Scenes.StartAppScene);

            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _soundsConfig = SoundsManager.Config;
        }

        [UsedImplicitly]
        public void PlaySound(int index)
        {
            var clip = _soundsConfig.SplashSounds[index];
            SoundsManager.PlaySound(clip);
        }

        [UsedImplicitly]
        public void CompleteAnimation()
        {
            _renderer.DOFade(0, 2).OnComplete(() => ScenesManager.LoadScene(ScenesManager.Scenes.MainMenuScene));
        }
    }
}