using HarmonyLib;

namespace LittleThings.Patches
{
    [HarmonyPatch(typeof(GPSTrigger), "Start")]
    internal class GPSTrigger__Start
    {
        [HarmonyPrefix]
        internal static bool GPSCheck()
        {
            //If GPS is enabled, go with this option in arcade. GPS Triggers will activate
            if (Mod.EnableGPSInArcade.Value)
            {
                return false;
            }
            return true;
        }
    }
}
