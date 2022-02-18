using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Coloring2.DataServices
{
    public class ConfigsService : AbstractService
    {
        private List<ScriptableObject> _configs = new List<ScriptableObject>();
        
        public ConfigsService() { }

        public ConfigsService AddConfig(ScriptableObject config)
        {
            if(!_configs.Contains(config))_configs.Add(config);
            return this;
        }
        
        public T GetConfig<T>() where T : ScriptableObject
        {
           return (T)_configs.FirstOrDefault(c => c is T);
        }
    }
}