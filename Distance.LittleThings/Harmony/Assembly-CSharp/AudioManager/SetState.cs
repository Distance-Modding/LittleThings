using HarmonyLib;

namespace Distance.LittleThings.Harmony
{
    [HarmonyPatch(typeof(AudioManager), "SetState", new System.Type[] { typeof(string), typeof(string) })]
    internal class AudioManager__SetState
    {
        [HarmonyPostfix]
        internal static void CustomAudioCheck(string stateGroup, string state)
        {
            if (Mod.Instance.Config.EnableCustomLowpass)
            {
                AudioManager audioManager;
                if (Mod.Instance.audioManager != null)
                    audioManager = Mod.Instance.audioManager;
                else
                    return;

                //Mod.Instance.Logger.Debug("Audio StateGroup String: " + stateGroup + "  Audio State String: " + state);
                if (audioManager.currentMusicState_ == AudioManager.MusicState.CustomMusic)
                {
                    if (state == "Under_Water")
                    {
                        if (audioManager.customMusicLowPass_ != null)
                            audioManager.StopCoroutine(audioManager.customMusicLowPass_);

                        audioManager.customMusicLowPass_ = audioManager.StartCoroutine(Mod.Instance.CustomMusicDSP(230f, .75f));
                        return;
                    }
                    if (state == "Normal")
                    {
                        if (stateGroup == "Water_States" || stateGroup == "GravityLowPass")
                        {
                            if (audioManager.customMusicLowPass_ != null)
                                audioManager.StopCoroutine(audioManager.customMusicLowPass_);

                            audioManager.customMusicLowPass_ = audioManager.StartCoroutine(Mod.Instance.CustomMusicDSP(AudioManager.lowPassFreqDefault_, .75f));
                        }
                    }
                }
            }
        }
        
    }
}
