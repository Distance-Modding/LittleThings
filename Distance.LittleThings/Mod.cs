using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Data;
using Centrifuge.Distance.GUI.Controls;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using UnityEngine;

namespace Distance.LittleThings
{
	/// <summary>
	/// The mod's main class containing its entry point
	/// </summary>
	[ModEntryPoint("Little Things")]
	public sealed class Mod : MonoBehaviour
	{
        public static Mod Instance { get; private set; }

        public IManager Manager { get; private set; }

        public Log Logger { get; private set; }

        public ConfigLogic Config { get; private set; }

        /// <summary>
        /// Method called as soon as the mod is loaded.
        /// WARNING:	Do not load asset bundles/textures in this function
        ///				The unity assets systems are not yet loaded when this
        ///				function is called. Loading assets here can lead to
        ///				unpredictable behaviour and crashes!
        /// </summary>
        public void Initialize(IManager manager)
		{
			// Do not destroy the current game object when loading a new scene
			DontDestroyOnLoad(this);

			Instance = this;

			Manager = manager;

            Config = gameObject.AddComponent<ConfigLogic>();

            //Check whether or not leaderboard uploads can happen
            OnConfigChanged(Config);

            //Subcribe to config event
            Config.OnChanged += OnConfigChanged;

            // Create a log file
            Logger = LogManager.GetForCurrentAssembly();

			Logger.Info("Thanks for using the Little Things ya rascal!");

            try
            {
                CreateSettingsMenu();
            }
            catch (Exception e)
            {
                Logger.Exception(e);
                Logger.Error("This likely happened because you have the wrong version of Centrifuge.Distance. \nTo fix this, be sure to use the Centrifuge.Distance.dll file that came included with the mod's zip file. \nDespite this error, the mod will still function, however, you will not have access to the mod's menu.");
            }

            try
            {
                // Never ever EVER use this!!!
                // It's the same as below (with `GetCallingAssembly`) wrapped around a silent catch-all.
                //RuntimePatcher.AutoPatch();

                RuntimePatcher.HarmonyInstance.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Logger.Error("Little Things: Error during Harmony.PatchAll()");
                Logger.Exception(ex);
                throw;
            }
        }

        private void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.littlethings", "Little Thing Settings")
            {
                new CheckBox(MenuDisplayMode.Both, "settings::gps_arcade", "ENABLE MINIMAP IN ARCADE")
                .WithGetter(() => Config.EnableGPSInArcade)
                .WithSetter((x) => Config.EnableGPSInArcade = x)
                .WithDescription("Toggles whether Minimap will become available to use in Arcade Mode. (GPS Triggers will also activate in arcade)"),
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "LITTLE THINGS", "Settings for the LittleThings mod");
        }

		public void OnConfigChanged(ConfigLogic configLogic)
        {

        }
	}
}



