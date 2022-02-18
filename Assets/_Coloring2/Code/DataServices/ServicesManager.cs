using System.Collections.Generic;
using System.Linq;

namespace Coloring2.DataServices
{
    public class ServicesManager
    {
        private static List<IDataService> _services = new List<IDataService>();
            
        public static T Register<T>(IDataService service)where T : AbstractService
        {
            if (_services.Contains(service)) 
                return null;
            _services.Add(service);
            return (T)service;
        }
        
        public static void Unregister(IDataService service)
        {
            if(_services.Contains(service))
                _services.Remove(service);
        }

        public static void Dispose()
        {
            _services.ForEach(s => s.Dispose());
        }

        public static T GetService<T>() where T : IDataService
        {
            return (T) _services.FirstOrDefault(s => s is T);
        }
    }
}