using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] string mixerSfxFieldName;
    [SerializeField] string playerPrefsSfxName;

    bool toggled = true;

    void Start()
    {
        int soundOn = PlayerPrefs.GetInt(playerPrefsSfxName, 1);
        SetAudio(soundOn != 0);
    }

    public void SetAudio(bool toggle)
    {
        if (toggled == toggle)
            return;
        toggled = toggle;

        mixer.SetFloat(mixerSfxFieldName, toggle ? 0f : -80f);
        PlayerPrefs.SetInt(playerPrefsSfxName, toggle ? 1 : 0);
    }
}
