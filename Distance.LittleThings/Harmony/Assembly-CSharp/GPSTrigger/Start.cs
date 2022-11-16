using HarmonyLib;

namespace Distance.LittleThings.Harmony
{
    [HarmonyPatch(typeof(GPSTrigger), "Start")]
    internal class GPSTrigger__Start
    {
        [HarmonyPrefix]
        internal static bool GPSCheck()
        {
            //If GPS is enabled, go with this option in arcade. GPS Triggers will activate
            if(Mod.Instance.Config.EnableGPSInArcade)
            {
                return false;
            }
            return true;
        }
    }
}
