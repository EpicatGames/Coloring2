using System.Collections.Generic;

namespace Coloring2.DataServices
{
    public class SelectedItemService : AbstractService
    {
        private ISelectable _selected;
        
        public void Set(ISelectable item)
        {
            _selected = item;
        }
        
        public T Get<T>() where T : ISelectable
        {
            var result = (T) _selected;
            _selected = null;
            return result;
        }
    }
}