using HarmonyLib;

namespace LittleThings.Patches
{
    [HarmonyPatch(typeof(QuarantineTrigger), "Start")]
    internal class QuarantineTrigger__Start
    {
        [HarmonyPrefix]
        internal static bool StartRewrite(QuarantineTrigger __instance)
        {
            //Get rid of the Campaign mode check in the Start function for QuarantineZones
            if (Mod.EnableQuarantineInArcade.Value)
            {
                UnityStandardAssets.ImageEffects.Bloom componentInChildren = __instance.cameraPrefab_.GetComponentInChildren<UnityStandardAssets.ImageEffects.Bloom>();
                if (componentInChildren == null)
                    return false;
                __instance.originalBloomIntensity_ = componentInChildren.bloomIntensity;
                return false;
            }
            return true;
        }
    }
}
