using System.Runtime.CompilerServices;
using Coloring2.Configs;
using Coloring2.DataServices;
using DG.Tweening;
using UnityEngine;

namespace Coloring2
{
    public class SoundsManager
    {
        public static AudioConfig Config { get; private set; }
        
        private static SoundsManager _instance;
        private static AudioSource _sndSource;
        private static AudioSource _bgMusicSource;
        
        private static bool _soundOn = true;
        private static bool _musicOn = true;

        public static bool SoundOn
        {
            get => _soundOn;
            set
            {
                _soundOn = value;
                _sndSource.volume = _soundOn ? 1 : 0;
            }
        }

        public static bool MusicOn
        {
            get => _musicOn = true;
            set
            {
                _musicOn = value;
                DOTween.Kill(_bgMusicSource);
                if (!_musicOn)
                {
                    _bgMusicSource.DOFade(0, .8f).OnComplete(_bgMusicSource.Pause);
                }
                else
                {
                    _bgMusicSource.Play();
                    _bgMusicSource.DOFade(1, .8f);
                }
            }
        }

        public static SoundsManager Create()
        {
            if (_instance != null)
                return _instance;

            _instance = new SoundsManager();
            return _instance;
        }

        private SoundsManager()
        {
            var configsService = ServicesManager.GetService<ConfigsService>();
            Config = configsService.GetConfig<AudioConfig>();
            
            var audioSource = new GameObject("AudioComponents");
            Object.DontDestroyOnLoad(audioSource);
            
            _sndSource = audioSource.AddComponent<AudioSource>();
            _sndSource.playOnAwake = false;
            _bgMusicSource = audioSource.AddComponent<AudioSource>();
            _bgMusicSource.playOnAwake = false;
            _bgMusicSource.loop = true;
        }
        
        public static void PlayBackgroundMusic()
        {
            if(!_musicOn)
                return;
            
            _bgMusicSource.clip = Config.BgMusic;
            _bgMusicSource.Play();
        }
        
        public static void PlaySound(AudioClip sound)
        {
            if(!_soundOn)
                return;
            
            _sndSource.clip = sound;
            _sndSource.Play();
        }

        public static void PlayClick() => PlaySound(Config.Click);

        public static void PlayCorrectBirthYear() => PlaySound(Config.CorrectBirthYear);

        public static void PlayPickCategory() => PlaySound(Config.Click);

        public static void PlayLineEnd()
        {
            if(!_soundOn)
                return;
            _sndSource.PlayOneShot(Config.LineEnd, 0.2f);
        }

        public static void PlayPhoto()
        {
            if(!_soundOn)
                return;
            PlaySound(Config.Photo);
        }

        public static void PlaySwapCategory()
        {
            if(!_soundOn)
                return;
            _sndSource.PlayOneShot(Config.SwapCategory, 4f);
        }
    }
}