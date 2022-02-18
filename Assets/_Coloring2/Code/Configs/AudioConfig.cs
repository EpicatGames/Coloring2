using System;
using UnityEngine;

namespace Coloring2.Configs
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Coloring2/AudioConfig", order = 0)]
    public class AudioConfig : ScriptableObject
    {
        public AudioClip Click;
        public AudioClip LineEnd;
        public AudioClip Photo;
        public AudioClip PickCategory;
        public AudioClip SwapCategory;
        public AudioClip CorrectBirthYear;
        public AudioClip BgMusic;
        
        [Space(10)]
        public AudioClip[] SplashSounds;
    }
}