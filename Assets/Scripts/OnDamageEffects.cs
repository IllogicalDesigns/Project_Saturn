using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamageEffects : MonoBehaviour
{
    [SerializeField] AudioClip universalHitMarker;
    [SerializeField] AudioSource source;

    public void OnDamage() {
        //FreezeFrame
        source.spatialBlend = 0;
        source.PlayOneShot(universalHitMarker);
    }
}
