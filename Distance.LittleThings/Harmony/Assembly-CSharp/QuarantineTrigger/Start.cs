using HarmonyLib;

namespace Distance.LittleThings.Harmony
{
    [HarmonyPatch(typeof(QuarantineTrigger), "Start")]
    internal class Start
    {
        [HarmonyPrefix]
        internal static bool StartRewrite(QuarantineTrigger __instance)
        {
            //Get rid of the Campaign mod check in the Start function for QuarantineZones
            if (Mod.Instance.Config.EnableQuarantineInArcade)
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
