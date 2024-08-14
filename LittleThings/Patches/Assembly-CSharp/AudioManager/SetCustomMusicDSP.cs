using HarmonyLib;

namespace LittleThings.Patches
{
    [HarmonyPatch(typeof(AudioManager), "SetCustomMusicDSP", new System.Type[] { typeof(float), typeof(float), typeof(bool) })]
    internal class AudioManaher__SetCustomMusicDSP
    {
        [HarmonyPostfix]
        internal static void CheckingIfItIsCalled(float lowPassFreq, float highPassFreq, bool forceUpdate)
        {
            //Mod.Log.LogInfo("DSP Freaks: " + lowPassFreq + " " + highPassFreq);
        }
    }
}
