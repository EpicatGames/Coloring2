namespace Coloring2
{
    public class AnalyticsDisabler
    {
        public AnalyticsDisabler()
        {
            UnityEngine.Analytics.Analytics.enabled = false;
            UnityEngine.Analytics.Analytics.deviceStatsEnabled = false;
            UnityEngine.Analytics.Analytics.initializeOnStartup = false;
            UnityEngine.Analytics.Analytics.limitUserTracking = false;
            UnityEngine.Analytics.PerformanceReporting.enabled = false;
        }
    }
}

