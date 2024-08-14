using HarmonyLib;

namespace LittleThings.Patches
{
    [HarmonyPatch(typeof(CarScreenLogic), "OnEventGo")]
    internal class CarScreenLogic__OnEventGo
    {
        [HarmonyPrefix]
        internal static bool DisablePlacementText(CarScreenLogic __instance)
        {
            if (Mod.ActiveCompass.Value)
            {
                //Literally just skip the method if active compass is on
                __instance.ModeWidgetVisible_ = true;
                return false;
            }
            else
                return true;
        }
    }
}
