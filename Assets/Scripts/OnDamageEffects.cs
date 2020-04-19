using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamageEffects : MonoBehaviour
{
    [SerializeField] AudioClip universalHitMarker, MeleePunch;
    [SerializeField] AudioSource source;

    public void OnDamage() {
        //FreezeFrame
        source.spatialBlend = 0;
        source.PlayOneShot(universalHitMarker);
    }

    public void OnMeleeDamage() {
        //FreezeFrame
        source.spatialBlend = 0;
        source.PlayOneShot(MeleePunch);
    }
}
