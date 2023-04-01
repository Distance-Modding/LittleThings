using HarmonyLib;

namespace Distance.LittleThings.Harmony
{
    [HarmonyPatch(typeof(AudioManager), "SetRTPCValue", new System.Type[] { typeof(string), typeof(float) })]
    internal class AudioManager__SetRTPCValue
    {
        [HarmonyPostfix]
        internal static void AdjustCustomRTPC(string rtpcName, float value)
        {
            if (Mod.Instance.Config.EnableCustomLowpass)
            {
                AudioManager audioManager;
                if (Mod.Instance.audioManager != null)
                    audioManager = Mod.Instance.audioManager;
                else
                    return;


                //Mod.Instance.Logger.Debug(rtpcName + " " + value);
                if (audioManager.currentMusicState_ == AudioManager.MusicState.CustomMusic)
                {
                    if (rtpcName == "TunnelHorror")
                    {
                        if (value != 0)
                        {

                            audioManager.SetCustomMusicDSP(AudioManager.lowPassFreqDefault_ / (float)System.Math.Pow(10.0, value), AudioManager.highPassFreqDefault_, false);
                        }
                        else
                        {
                            //Hoping there's no popping because of this
                            //For whatever reason this isn't guarenteed?
                            audioManager.SetCustomMusicDSP(AudioManager.lowPassFreqDefault_, AudioManager.highPassFreqDefault_);
                        }
                    }
                }
            }
        }
    }
}
