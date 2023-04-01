using HarmonyLib;

namespace Distance.LittleThings.Harmony
{
    [HarmonyPatch(typeof(AudioManager), "Update")]
    internal class AudioManager__Update
    {
        [HarmonyPostfix]
        internal static void LiterallyJustGettingTheAudioManager(AudioManager __instance)
        {
            Mod.Instance.audioManager = __instance;
            //Mod.Instance.Logger.Debug("DSP Freaks: " + __instance.lowPassFreq_ + " " + __instance.highPassFreq_);
        }
    }
}
