using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class ApplyDamage : MonoBehaviour {
        [SerializeField] public string tagToDamgage = "Player";
        [SerializeField] private bool applyKnockBack = false;
        [SerializeField] private int dmg = 25;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag(tagToDamgage)) {
                other.SendMessage("ApplyDamage", dmg);
                if(applyKnockBack)
                    other.SendMessage("ApplyKnockbackVec3", transform.position);
            }
        }
    }
}