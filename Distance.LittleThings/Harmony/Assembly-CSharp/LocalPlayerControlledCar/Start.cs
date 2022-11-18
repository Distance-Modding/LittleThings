using HarmonyLib;

namespace Distance.LittleThings.Harmony
{
    [HarmonyPatch(typeof(LocalPlayerControlledCar), "Start")]
    internal class LocalPlayerControlledCar__Start
    {
        [HarmonyPostfix]
        internal static void HeadlightsCheck(LocalPlayerControlledCar __instance)
        {
            if (Mod.Instance.Config.EnableHeadLights)
            {
                //Hardcoded cause I'm cringe. Didn't feel like building menu values to manipulate
                __instance.carLogic_.carVisuals_.SetHeadlightsValues(1f, 100f, 150f, .5f, false);
            }
        }
    }
}
