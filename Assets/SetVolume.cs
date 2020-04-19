using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] Slider slider;
    public AudioMixer mixer;

    public void SetLevel(float sliderValue) {
        mixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Volume", sliderValue);
    }

    private void Start() {
        slider = gameObject.GetComponent<Slider>();
        float volume = PlayerPrefs.GetFloat("Volume", 0);
        SetLevel(volume);
        slider.value = volume;
    }
}
