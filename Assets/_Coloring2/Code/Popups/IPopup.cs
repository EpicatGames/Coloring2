using System;
using UnityEngine;

namespace Coloring2.Popups
{
    public interface IPopup
    {
        event Action<IPopup> Closed;
        void Show();
        GameObject gameObject { get; }
    }
}

