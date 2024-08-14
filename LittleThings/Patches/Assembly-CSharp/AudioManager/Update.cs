using HarmonyLib;

namespace LittleThings.Patches
{
    [HarmonyPatch(typeof(AudioManager), "Update")]
    internal class AudioManager__Update
    {
        [HarmonyPostfix]
        internal static void LiterallyJustGettingTheAudioManager(AudioManager __instance)
        {
            Mod.Instance.audioManager = __instance;
            //Mod.Log.LogInfo("DSP Freaks: " + __instance.lowPassFreq_ + " " + __instance.highPassFreq_);
        }
    }
}
