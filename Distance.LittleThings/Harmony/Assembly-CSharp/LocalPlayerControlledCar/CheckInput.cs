using HarmonyLib;
using UnityEngine;


namespace Distance.LittleThings.Harmony
{
    [HarmonyPatch(typeof(LocalPlayerControlledCar), "CheckInput")]
    internal class CheckInput
    {
        [HarmonyPrefix]
        internal static bool GPSCheck(LocalPlayerControlledCar __instance, InputStates inputStates, float dt)
        {
            if (Mod.Instance.Config.EnableGPSInArcade)
            {
                if ((Object)__instance.playerDataLocal_ == (Object)null || !__instance.playerDataLocal_.CarInputEnabled_)
                    return false;
                CarDirectives carDirectives = __instance.carLogic_.CarDirectives_;
                carDirectives.ZeroThis();
                float num1 = inputStates.GetState(InputAction.Gas).value_;
                carDirectives.Gas_ = num1;
                if (__instance.BoostEnabled_)
                {
                    InputState state = inputStates.GetState(InputAction.Boost);
                    bool invertBoost = __instance.playerDataLocal_.Profile_.InvertBoost_;
                    float num2 = !invertBoost ? state.value_ : 1f - state.value_;
                    bool flag = !invertBoost ? state.isPressed_ : !state.isPressed_;
                    if ((double)num1 > 0.0 && (double)num1 == (double)num2)
                    {
                        carDirectives.Gas_ = Mathf.Min(num1 * 2f, 1f);
                        carDirectives.Boost_ = (double)num2 > 0.899999976158142;
                    }
                    else
                    {
                        carDirectives.Boost_ = flag;
                        if (carDirectives.Boost_)
                            carDirectives.Gas_ = 1f;
                    }
                }
                carDirectives.Brake_ = inputStates.GetState(InputAction.Brake).value_;
                if (!__instance.WingsActive_ && __instance.JetsEnabled_)
                {
                    carDirectives.Yaw_ = 0.0f;
                    if (inputStates.GetState(InputAction.Grip).isPressed_)
                        carDirectives.Grip_ = true;
                    if (carDirectives.Grip_ || __instance.carStats_.WheelsContacting_ == 0 && !__instance.JumpPossible_)
                    {
                        carDirectives.Pitch_ = inputStates.GetDualAxis(InputActionPair.AirPitch);
                        carDirectives.Roll_ = inputStates.GetDualAxis(InputActionPair.AirRoll);
                        if (inputStates.GetPressed(InputAction.AirRollLeft) && inputStates.GetPressed(InputAction.AirRollRight) || inputStates.GetPressed(InputAction.AirPitchDown) && inputStates.GetPressed(InputAction.AirPitchUp))
                            carDirectives.Grip_ = true;
                    }
                    carDirectives.Pitch_ += inputStates.GetDualAxis(InputActionPair.JetPitch);
                    carDirectives.Roll_ += inputStates.GetDualAxis(InputActionPair.JetRoll);
                    if (inputStates.GetPressed(InputAction.JetRollLeft) && inputStates.GetPressed(InputAction.JetRollRight) || inputStates.GetPressed(InputAction.JetPitchDown) && inputStates.GetPressed(InputAction.JetPitchUp))
                        carDirectives.Grip_ = true;
                    float f = MathEx.Sq(carDirectives.Pitch_) + MathEx.Sq(carDirectives.Roll_);
                    if ((double)f > 1.0)
                    {
                        float num3 = 1f / Mathf.Sqrt(f);
                        carDirectives.Pitch_ *= num3;
                        carDirectives.Roll_ *= num3;
                    }
                }
                float num4 = __instance.playerDataLocal_.Profile_.TurningSensitivity_ + 0.5f;
                carDirectives.Steer_ = inputStates.GetDualAxis(InputActionPair.Steer) * num4;
                if (!__instance.ignoreSteeringAdjustments_)
                    carDirectives.Steer_ *= __instance.CalcVelocityBasedSteeringMultiplier();
                if (__instance.playerDataLocal_.EnableShowScores_ && inputStates.GetState(InputAction.ShowScore).isPressed_ && __instance.gameMan_.IsModeCreated_)
                {
                    __instance.ShowPlacementText();
                    if (__instance.playerDataLocal_.CarScreenLogic_ != null)
                    {
                        //This is to prevent the bug that happens with the skeleton car. This also fixes it for Custom Cars.
                        __instance.playerDataLocal_.CarScreenLogic_.ShowMinimap(MinimapMode.ButtonPress, showDestText: true, useCrosshair: (G.Sys.GameManager_.ModeID_ == GameModeID.Nexus));
                    }
                }
                __instance.UpdateGasBrake(dt);
                if (!__instance.gameMan_.SoloAndNotOnline_)
                    return false;
                if (inputStates.GetPressed(InputAction.Reset))
                {
                    __instance.resetHoldTimer_ += dt;
                    if ((double)__instance.resetHoldTimer_ <= (double)__instance.resetHoldTime_)
                        return false;
                    __instance.gameMan_.RestartLevel();
                }
                else
                    __instance.resetHoldTimer_ = 0.0f;

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
