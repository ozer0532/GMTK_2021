using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    public AudioMixer mixer;
    public Sprite onSprite;
    public Sprite offSprite;
    public Image image;

    private bool isOn = true;

    public void Toggle()
    {
        print(isOn);
        isOn = !isOn;
        if (isOn) On();
        else Off();
    }

    public void On()
    {
        mixer.SetFloat("Music/Volume", 0f);
        mixer.SetFloat("SFX/Volume", 0f);
        image.sprite = onSprite;
    }

    public void Off()
    {
        mixer.SetFloat("Music/Volume", -100f);
        mixer.SetFloat("SFX/Volume", -100f);
        image.sprite = offSprite;
    }
}
