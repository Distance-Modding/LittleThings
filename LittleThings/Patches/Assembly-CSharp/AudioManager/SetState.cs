using HarmonyLib;

namespace LittleThings.Patches
{
    [HarmonyPatch(typeof(AudioManager), "SetState", new System.Type[] { typeof(string), typeof(string) })]
    internal class AudioManager__SetState
    {
        [HarmonyPostfix]
        internal static void CustomAudioCheck(string stateGroup, string state)
        {
            if (Mod.EnableCustomLowpass.Value)
            {
                AudioManager audioManager;
                if (Mod.Instance.audioManager != null)
                    audioManager = Mod.Instance.audioManager;
                else
                    return;

                //Mod.Log.LogInfo("Audio StateGroup String: " + stateGroup + "  Audio State String: " + state);
                if (audioManager.currentMusicState_ == AudioManager.MusicState.CustomMusic)
                {
                    if (state == "Under_Water")
                    {
                        if (audioManager.customMusicLowPass_ != null)
                            audioManager.StopCoroutine(audioManager.customMusicLowPass_);

                        audioManager.customMusicLowPass_ = audioManager.StartCoroutine(Mod.Instance.CustomMusicDSP(230f, .75f));
                        return;
                    }

                    if (state == "NoGravity")
                    {
                        if (audioManager.customMusicLowPass_ != null)
                            audioManager.StopCoroutine(audioManager.customMusicLowPass_);

                        audioManager.customMusicLowPass_ = audioManager.StartCoroutine(Mod.Instance.CustomMusicDSP(2000f, .75f));
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
