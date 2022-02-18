using UnityEngine;

namespace Coloring2.DataServices
{
    public class PlayerStatisticService : AbstractService
    {
        public int NumberOfLaunches { get; private set;}
        
        public PlayerStatisticService()
        {
            var numLauncherKey = "number_of_launches";
            if (!PlayerPrefs.HasKey(numLauncherKey))
                PlayerPrefs.SetInt(numLauncherKey, 1);
            else
                PlayerPrefs.SetInt(numLauncherKey, PlayerPrefs.GetInt(numLauncherKey) + 1);

            NumberOfLaunches = PlayerPrefs.GetInt(numLauncherKey);
        }
    }
}