using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatusCues : MonoBehaviour
{
    [SerializeField] Slider bloodSlider, hpSlider;
    [SerializeField] AudioSource bloodSrc, hpSrc;

    // Update is called once per frame
    void Update()
    {
        if (bloodSrc.isPlaying) {
            bloodSrc.pitch = (100 - bloodSlider.value) * 0.01f;
            bloodSrc.volume = (100 - bloodSlider.value) * 0.01f;
        }

        if (hpSrc.isPlaying) {
            hpSrc.pitch = (100 - hpSlider.value) * 0.015f;
            hpSrc.volume = (100 - hpSlider.value) * 0.015f;
        }

        if (bloodSlider.value < 25f  && !bloodSrc.isPlaying) {
            bloodSrc.Play();
        }
        else if(bloodSrc.isPlaying) {
            bloodSrc.Stop();
        }

        if (hpSlider.value < 25f && !hpSrc.isPlaying) {
            hpSrc.Play();
        }
        else if (hpSrc.isPlaying) {
            hpSrc.Stop();
        }
    }
}
