using System;
using System.Threading;
using Coffee.UIEffects;
using Coffee.UIExtensions;
using Coloring2.Configs;
using Coloring2.DataServices;
using Coloring2.Localization;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using ntw.CurvedTextMeshPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Coloring2.MainMenu.Categories
{
    public class MenuCategory : MonoBehaviour
    {
        private const float letterDefDist = 15;
        private const float letterOffset = 5;
            
        [SerializeField] private CategoryConfig _config;
        [SerializeField] private TextMeshProUGUI _nameField;
        [SerializeField] private Text _priceField;
        [SerializeField] private UIEffect _uiEffect;
        [SerializeField] private GameObject _lockIcon;
        [SerializeField] private GameObject _flyingContainer;
        [SerializeField] private ParticleSystem _particleSystem;

        private int _screenWidth;
        
        private TextProOnACircle _textOnCircle;
        private CanvasGroup _canvasGroup;
        private AnimationCurve _scaleCurve;
        private AnimationCurve _alphaCurve;
        private TweenerCore<Vector2, Vector2, VectorOptions> _flyingTween;
        private CancellationTokenSource _cancellationToken;

        public CategoryConfig Config => _config;
        
        public Vector2 Size { get; private set; }

        public RectTransform RectTransform { get; private set; }
        
        private void Awake()
        {
            _textOnCircle = _nameField.GetComponent<TextProOnACircle>();
            _canvasGroup = GetComponent<CanvasGroup>();
            RectTransform = GetComponent<RectTransform>();
            Size = RectTransform.sizeDelta;
        }

        private void OnDestroy()
        {
            _flyingTween?.Kill();
            _cancellationToken?.Cancel();
        }

        public void Init(AnimationCurve scaleCurve, AnimationCurve alphaCurve)
        {
            _scaleCurve = scaleCurve;
            _alphaCurve = alphaCurve;
            _screenWidth = Screen.width;
            _nameField.color = _config.Color;
            
            var str = LocalizationManager.GetLocalization(_config.Category.ToString());
            _nameField.text = str;

            _priceField.text = PlayerPurchasesService.GetPriceString(Config.Category);

            var currDeg = letterDefDist;
            for (var i = 0; i < str.Length - 1; i++)
                currDeg += letterOffset;
            _textOnCircle.ArcDegrees = -currDeg;

            var interactor = GetComponentInChildren<CategoryInteractor>();
            interactor.Init(this);

            CheckPurchase();
            
            _cancellationToken = new CancellationTokenSource();
            UniTask.Delay(TimeSpan.FromSeconds(Random.Range(0, 2f)), cancellationToken: _cancellationToken.Token)
                .ContinueWith(StartFlyingBehaviour);
        }
        
        public void Activate()
        {
            _uiEffect.effectFactor = 0;
            _lockIcon.SetActive(false);
            if (!_particleSystem.isPlaying)
            {
                _particleSystem.GetComponent<ParticleSystemRenderer>().sharedMaterial = Config.ParticlesMaterial;
                _particleSystem.Play();
            }
        }

        private void StartFlyingBehaviour()
        {
            var flyingRect = _flyingContainer.GetComponent<RectTransform>();
            _flyingTween = flyingRect.DOAnchorPosY(Size.y * .1f, 3f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void CheckPurchase()
        {
            var purchaseService = ServicesManager.GetService<PlayerPurchasesService>();
            var purchased = purchaseService.HasCategoryPurchased(Config.Category);
            if (!purchased)
                _uiEffect.effectFactor = 1f;
            else
                Activate();
        }

        public void UpdateScaleAndOpacity()
        {
            var percent = transform.position.x / _screenWidth;
            if(percent < -0.5f || percent > 1.5f)
                return;

            var scale = _scaleCurve.Evaluate(percent);
            transform.localScale = new Vector3(scale, scale, scale);
            var alpha = _alphaCurve.Evaluate(percent);
            _canvasGroup.alpha = alpha;
        }

        public override string ToString()
        {
            return $"[Name: {gameObject.name}, Category:{Config.Category}]";
        }
    }
}