using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;

namespace LittleThings
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public sealed class Mod : BaseUnityPlugin
    {
        //Mod Details
        private const string modGUID = "Distance.LittleThings";
        private const string modName = "Little Things";
        private const string modVersion = "4.1";

        //Config Entry Strings
        public static string ActiveCompassKey = "Permanent Compass";
        public static string EnableLowpassKey = "Enable Custom Lowpass Filter";
        public static string EnableGPSKey = "Enable GPS in Arcade";
        public static string EnableHeadLightsKey = "Enables Headlights";
        public static string EnableQuarantineKey = "Enables Quarantine Zones in Arcade";

        //Config Entries
        public static ConfigEntry<bool> ActiveCompass { get; set; }
        public static ConfigEntry<bool> EnableCustomLowpass { get; set; }
        public static ConfigEntry<bool> EnableGPSInArcade { get; set; }
        public static ConfigEntry<bool> EnableHeadLights { get; set; }
        public static ConfigEntry<bool> EnableQuarantineInArcade { get; set; }

        //Public Variables
        public AudioManager audioManager { get; set; }

        //Other
        private static readonly Harmony harmony = new Harmony(modGUID);
        public static ManualLogSource Log = new ManualLogSource(modName);
        public static Mod Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            Logger.LogInfo("Thanks for using Little Things!");

            //Config Setup
            ActiveCompass = Config.Bind("General",
                ActiveCompassKey,
                false,
                new ConfigDescription("The compass will always stay active on the carscreen and never change"));

            EnableCustomLowpass = Config.Bind("General",
                EnableLowpassKey,
                false,
                new ConfigDescription("Toggles whether or not lowpass filters get applied to custom music."));

            EnableGPSInArcade = Config.Bind("General",
                EnableGPSKey,
                false,
                new ConfigDescription("Toggles whether Minimap will become available to use in Arcade Mode. (GPS Triggers will also activate in arcade)"));

            EnableHeadLights = Config.Bind("General",
                EnableHeadLightsKey,
                false,
                new ConfigDescription("Toggles whether head lights are always active on the car, just like the Beta days!"));

            EnableQuarantineInArcade = Config.Bind("General",
                EnableQuarantineKey,
                false,
                new ConfigDescription("Toggles whether Quarantine zones will activate in arcade mode"));

            ActiveCompass.SettingChanged += OnConfigChanged;
            EnableCustomLowpass.SettingChanged += OnConfigChanged;
            EnableGPSInArcade.SettingChanged += OnConfigChanged;
            EnableHeadLights.SettingChanged += OnConfigChanged;
            EnableQuarantineInArcade.SettingChanged += OnConfigChanged;

            //Apply Patches
            Logger.LogInfo("Loading...");
            harmony.PatchAll();
            Logger.LogInfo("Loaded!");
        }

        private void OnConfigChanged(object sender, EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            if (settingChangedEventArgs == null) return;
        }

        public System.Collections.IEnumerator CustomMusicDSP(float lowpassEnd, float timer)
        {
            if (audioManager == null)
                yield break;

            if (audioManager.sampleAggregator_ == null || !audioManager.audioSettings_.AffectedByGameplay_)
                yield break;

            if (audioManager.lowPassFreq_.ApproxEquals(lowpassEnd))
                yield break;

            float startLowFreq = (float)Math.Log10((double)audioManager.lowPassFreq_);
            float endLowFreq = (float)Math.Log10((double)lowpassEnd);
            float time = 0f;
            while (time < timer)
            {
                audioManager.lowPassFreq_ = (float)Math.Pow(10.0, (double)UnityEngine.Mathf.Lerp(startLowFreq, endLowFreq, time / timer));
                audioManager.SetCustomMusicDSP(audioManager.lowPassFreq_, audioManager.highPassFreq_, false);
                time += UnityEngine.Time.deltaTime;
                yield return null;
            }
            audioManager.SetCustomMusicDSP((float)Math.Pow(10.0, (double)endLowFreq), -1f, false);
            yield break;
        }

        public System.Collections.IEnumerator EMPCusmtomMusicDSP()
        {

            //Hardcoded values for this one.
            if (audioManager == null)
                yield break;

            if (audioManager.sampleAggregator_ == null || !audioManager.audioSettings_.AffectedByGameplay_)
                yield break;

            float startLowFreq = (float)Math.Log10((double)audioManager.lowPassFreq_);
            float midLowFreq = (float)Math.Log10((double).01);
            float time1 = 0f;
            float time2 = 0f;

            while (time1 < .5f)
            {
                audioManager.lowPassFreq_ = (float)Math.Pow(10.0, (double)UnityEngine.Mathf.Lerp(startLowFreq, midLowFreq, time1 / .5f));
                audioManager.SetCustomMusicDSP(audioManager.lowPassFreq_, audioManager.highPassFreq_, false);
                time1 += UnityEngine.Time.deltaTime;
                yield return null;
            }

            while (time2 < 2f)
            {
                audioManager.lowPassFreq_ = (float)Math.Pow(10.0, (double)UnityEngine.Mathf.Lerp(midLowFreq, AudioManager.lowPassFreqDefault_, time2 / 2f));
                audioManager.SetCustomMusicDSP(audioManager.lowPassFreq_, audioManager.highPassFreq_, false);
                time2 += UnityEngine.Time.deltaTime;
                yield return null;
            }

            audioManager.SetCustomMusicDSP((float)Math.Pow(10.0, (double)AudioManager.lowPassFreqDefault_), -1f, false);
            yield break;
        } 
    }
}
