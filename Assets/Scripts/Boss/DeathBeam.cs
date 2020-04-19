using System;
using UnityEngine;

namespace Boss {
    public class DeathBeam : MonoBehaviour {

        [SerializeField] private float damage;
        [SerializeField] private float damageInterval;

        private float damageIntervalTracker;

        private void OnTriggerStay(Collider other) {
            if (!other.gameObject.CompareTag("Player") || damageIntervalTracker > 1e-4) return;
            
            other.gameObject.SendMessage("ApplyDamage", damage);
            damageIntervalTracker = damageInterval;
        }

        private void Update() {
            if (damageIntervalTracker > 1e-4) {
                damageIntervalTracker -= Time.deltaTime;
            }
        }
    }
}