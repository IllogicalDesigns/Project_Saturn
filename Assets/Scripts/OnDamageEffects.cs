using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamageEffects : MonoBehaviour
{
    [SerializeField] AudioClip universalHitMarker, MeleePunch;
    [SerializeField] AudioSource source;
    [SerializeField] SpriteFlipper flipper;

    public void OnDamage() {
        //FreezeFrame
        if (flipper != null) flipper.OnDamage();
        source.spatialBlend = 0;
        source.PlayOneShot(universalHitMarker);
    }

    public void OnMeleeDamage() {
        //FreezeFrame
        if (flipper != null) flipper.OnDamage();
        source.spatialBlend = 0;
        source.PlayOneShot(universalHitMarker);
        source.PlayOneShot(MeleePunch);
    }
}
