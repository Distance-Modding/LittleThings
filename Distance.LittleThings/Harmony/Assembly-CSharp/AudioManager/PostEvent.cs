using HarmonyLib;

namespace Distance.LittleThings.Harmony
{
    [HarmonyPatch(typeof(AudioManager), "PostEvent", new System.Type[] { typeof(string) })]
    internal class AudioManager__PostEvent1
    {
        [HarmonyPostfix]
        internal static void CheckCustomDSP(string eventName)
        {
            if (Mod.Instance.Config.EnableCustomLowpass)
            {
                AudioManager audioManager;
                if (Mod.Instance.audioManager != null)
                    audioManager = Mod.Instance.audioManager;
                else
                    return;

                //Mod.Instance.Logger.Debug("POST EVENT NAME: " + eventName);
                if (audioManager.currentMusicState_ == AudioManager.MusicState.CustomMusic)
                {
                    if (eventName == "Set_Master_Low_Pass_Fade_In")
                    {
                        if (audioManager.customMusicLowPass_ != null)
                            audioManager.StopCoroutine(audioManager.customMusicLowPass_);

                        audioManager.SetCustomMusicDSP(.01f, AudioManager.highPassFreqDefault_);
                        audioManager.customMusicLowPass_ = audioManager.StartCoroutine(Mod.Instance.CustomMusicDSP(AudioManager.lowPassFreqDefault_, 2f));
                        return;
                    }
                    if (eventName == "Set_Car_Low_Pass_Filter_Off")
                    {
                        audioManager.SetCustomMusicDSP(AudioManager.lowPassFreqDefault_, AudioManager.highPassFreqDefault_);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(AudioManager), "PostEvent", new System.Type[] { typeof(string), typeof(UnityEngine.GameObject) })]
    internal class AudioManager__PostEvent2
    {
        [HarmonyPostfix]
        internal static void CheckCustomDSP(string eventName)
        {
            //Mod.Instance.Logger.Debug("POST EVENT NAME: " + eventName);
        }
    }
}
