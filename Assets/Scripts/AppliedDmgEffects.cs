using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppliedDmgEffects : MonoBehaviour
{
    [SerializeField] AudioSource src;
    public void AppliedDmg() {
        src.Play();
    }
}
