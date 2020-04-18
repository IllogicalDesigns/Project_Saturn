using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour {
    [SerializeField] public string tagToDamgage = "Player";
    private int dmg = 25;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(tagToDamgage)) {
            other.SendMessage("ApplyDamage", dmg);
        }
    }
}