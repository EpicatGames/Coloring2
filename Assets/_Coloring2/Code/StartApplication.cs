using System;
using System.Collections;
using System.Collections.Generic;
using Coloring2.Configs;
using Coloring2.DataServices;
using Coloring2.Localization;
using Coloring2.Popups;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Coloring2
{
    public class StartApplication : MonoBehaviour
    {
        public static bool Initialized { get; private set; }
        
        private static StartApplication _instance;

        [SerializeField] private AudioConfig _audioConfig;
        [SerializeField] private AppConfig _appConfig;
        [SerializeField] private List<CategoryConfig> _categoryConfigs;

        private void Awake()
        {
            if(_instance != null)
                return;
            
            _instance = this;
            new AnalyticsDisabler();
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            ServicesManager.Dispose();
        }

        private void Start()
        {
            Init();
        }

        private async void Init()
        {
#if PLATFORM_ANDROID
            UnityEngine.Application.targetFrameRate = 30;
#else
            UnityEngine.Application.targetFrameRate = 60;
#endif
            var configsService = new ConfigsService();
            var projectContextService = new ProjectContextService();
            
            ServicesManager.Register<ConfigsService>(configsService)
                .AddConfig(_appConfig)
                .AddConfig(_audioConfig);
            
            await LocalizationManager.Initialize("Localization_Coloring2");
            
            CreateSoundsManager();
            ServicesManager.Register<PlayerPurchasesService>(new PlayerPurchasesService(_categoryConfigs));
            ServicesManager.Register<PlayerStatisticService>(new PlayerStatisticService());
            ServicesManager.Register<SelectedItemService>(new SelectedItemService());
            ServicesManager.Register<ProjectContextService>(projectContextService)
                .AddPopupsContainer(configsService)
                .AddScenesSwapScreen(configsService);
            
            ModalPopupsManager.Initialize(projectContextService.PopupsContainer);
            
            Initialized = true;
            
#if  UNITY_EDITOR
            var scene = (ScenesManager.Scenes) _appConfig.SceneToLoadAferStartApp.GetHashCode();
            ScenesManager.LoadScene(scene);
#else
            ScenesManager.LoadScene(ScenesManager.Scenes.SpashScreenScene);
#endif
        }

        private void CreateSoundsManager()
        {
            gameObject.AddComponent<AudioListener>();
            SoundsManager.Create();
        }
    }
}