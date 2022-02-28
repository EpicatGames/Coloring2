using System.Collections.Generic;

namespace Coloring2.DataServices
{
    public class SelectedCategoryService : AbstractService
    {
        private ISelectable _selected;
        
        public void Set(ISelectable item)
        {
            _selected = item;
        }
        
        public T Get<T>() where T : ISelectable
        {
            return (T) _selected;
        }
    }
}