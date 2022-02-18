using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Coloring2.Popups
{
    public static class ModalPopupsManager
    {
        public static IPopup Current { get; private set; }
        private static Transform _root;

        public static void Initialize(Transform root) => _root = root;

        public static T ShowPopup<T>(IPopup popupPrefab) where T : IPopup
        {
            ShowPopupInternal(popupPrefab);
            Current.Show();
            return (T) Current;
        }
        
        public static void ShowPopup(IPopup popupPrefab)
        {
            ShowPopupInternal(popupPrefab);
            Current.Show();
        }

        private static void ShowPopupInternal(IPopup popupPrefab)
        {
            if (Current != null)
                RemovePopup();

            if (_root == null)
                throw new Exception("PopupsManager does not initialized!");
        
            if (popupPrefab.gameObject.GetComponent<IPopup>() == null)
                throw new Exception("The popup prefab must have a script that implements the IPopup interface");
        
            Current = Object.Instantiate(popupPrefab.gameObject, _root).GetComponent<IPopup>();
        }

        public static void RemovePopup()
        {
            if(Current == null)
                return;
        
            Object.Destroy(Current.gameObject);
            Current = null;
        }
    }
}
