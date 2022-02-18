using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.CommonComponents
{
    [RequireComponent(typeof(Image))]
    public class ImageInfinityRotator : MonoBehaviour
    {
        [SerializeField] private float _time;
        [SerializeField] private Vector3 _axis;
        
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _tween;
        private bool _paused;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnDestroy()
        {
            Stop();
        }

        public void Begin()
        {
            if (_tween != null && _paused)
            {
                if (!_image.enabled)
                    _image.enabled = true;
                _tween.Play();
                return;
            }
            
            _tween = transform.DOLocalRotate(new Vector3(_axis.x, _axis.y, _axis.z), _time, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear).SetLoops(-1);
        }
        
        public void Pause(bool visible = true)
        {
            _tween.Pause();
            _paused = true;
            _image.enabled = visible;
        }
        
        public void Stop()
        {
            _tween.Kill();
        }
    }
}