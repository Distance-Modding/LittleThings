using HarmonyLib;

namespace LittleThings.Patches
{
    [HarmonyPatch(typeof(CarScreenLogic), "UpdateBeforeRender")]
    internal class CarScreenLogic__UpdateBeforeRender
    {
        [HarmonyPostfix]
        internal static void KeepCompassActive(CarScreenLogic __instance)
        {
            if (Mod.ActiveCompass.Value)
            {
                __instance.SetCurrentModeVisual(__instance.compass_);
            }
        }
    }
}
