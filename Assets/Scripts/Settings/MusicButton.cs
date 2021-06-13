using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicButton : MonoBehaviour
{
    public AudioMixer mixer;

    private bool isOn = true;

    public void Toggle()
    {
        isOn = !isOn;
        if (isOn) On();
        else Off();
    }

    public void On()
    {
        mixer.SetFloat("Music/Volume", 0f);
        mixer.SetFloat("SFX/Volume", 0f);
    }

    public void Off()
    {
        mixer.SetFloat("Music/Volume", -100f);
        mixer.SetFloat("SFX/Volume", -100f);
    }
}
