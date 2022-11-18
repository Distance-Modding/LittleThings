using HarmonyLib;

namespace Distance.LittleThings.Harmony
{
    [HarmonyPatch(typeof(CarScreenLogic), "UpdateBeforeRender")]
    internal class CarScreenLogic__UpdateBeforeRender
    {
        [HarmonyPostfix]
        internal static void KeepCompassActive(CarScreenLogic __instance)
        {
           if(Mod.Instance.Config.ActiveCompass)
           {
                __instance.SetCurrentModeVisual(__instance.compass_);
           }
        }
    }
}
