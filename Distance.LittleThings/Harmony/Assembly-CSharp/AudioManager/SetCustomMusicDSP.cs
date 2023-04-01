using HarmonyLib;

namespace Distance.LittleThings.Harmony
{
    [HarmonyPatch(typeof(AudioManager), "SetCustomMusicDSP", new System.Type[] { typeof(float), typeof(float), typeof(bool) })]
    internal class AudioManaher__SetCustomMusicDSP
    {
        [HarmonyPostfix]
        internal static void CheckingIfItIsCalled(float lowPassFreq, float highPassFreq, bool forceUpdate)
        {
            //Mod.Instance.Logger.Debug("DSP Freaks: " + lowPassFreq + " " + highPassFreq);
        }
    }
}
