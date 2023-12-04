using System;

namespace ElephantSDK
{
    [Serializable]
    public class InternalConfig
    {
        private static InternalConfig _instance;

        public bool monitoring_enabled;
        public bool crash_log_enabled;

        private InternalConfig()
        {
            monitoring_enabled = true;
            crash_log_enabled = false;
        }

        public static InternalConfig GetInstance()
        {
            return _instance ?? (_instance = new InternalConfig());
        }
    }
}