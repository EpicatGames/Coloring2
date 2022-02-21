using Coloring2.Configs;
using UnityEngine;

namespace Coloring2.DataServices
{
    public class ProjectContextService : AbstractService
    {
        public Transform PopupsContainer { get; private set; }
        public ScenesSwapScreen ScenesSwapScreen { get; private set; }
        
        public ProjectContextService() { }

        public ProjectContextService AddPopupsContainer(ConfigsService configsService)
        {
            var appConfig = configsService.GetConfig<AppConfig>();
            PopupsContainer = Object.Instantiate(appConfig.PopupsContainerRef);
            return this;
        }
        
        public ProjectContextService AddScenesSwapScreen(ConfigsService configsService)
        {
            var appConfig = configsService.GetConfig<AppConfig>();
            ScenesSwapScreen = Object.Instantiate(appConfig.ScenesSwapScreenRef);
            return this;
        }
    }
}