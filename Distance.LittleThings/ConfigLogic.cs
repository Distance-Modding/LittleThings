using System;
using Reactor.API.Configuration;
using UnityEngine;

namespace Distance.LittleThings
{
    public class ConfigLogic : MonoBehaviour
    {
        #region Properties
        public bool EnableGPSInArcade
        {
            get { return Get<bool>("EnableGPSInArcade"); }
            set { Set("EnableGPSInArcade", value); }
        }

        public bool EnableQuarantineInArcade
        {
            get { return Get<bool>("EnableQuarantineInArcade"); }
            set { Set("EnableQuarantineInArcade", value); }
        }
        #endregion

        internal Settings Config;

        public event Action<ConfigLogic> OnChanged;

        //Initialize Config
        private void Load()
        {
            Config = new Settings("Config");
        }

        public void Awake()
        {
            Load();
            //Setting Defaults
            Get("EnableGPSInArcade", true);
            Get("EnableQuarantineInArcade", true);
            //Save settings to Config.json
            Save();
        }

        public T Get<T>(string key, T @default = default(T))
        {
            return Config.GetOrCreate(key, @default);
        }

        public void Set<T>(string key, T value)
        {
            Config[key] = value;
            Save();
        }

        public void Save()
        {
            Config?.Save();
            OnChanged?.Invoke(this);
        }
    }
}
