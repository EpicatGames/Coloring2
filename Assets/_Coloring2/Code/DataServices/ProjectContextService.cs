using Coloring2.Configs;
using UnityEngine;

namespace Coloring2.DataServices
{
    public class ProjectContextService : AbstractService
    {
        public Transform PopupsContainer { get; private set; }
        
        public ProjectContextService() { }

        public void AddPopupsContainer(ConfigsService configsService)
        {
            var appConfig = configsService.GetConfig<AppConfig>();
            PopupsContainer = Object.Instantiate(appConfig.PopupsContainerRef);
        }
    }
}