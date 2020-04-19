using System;
using UnityEngine;

namespace Player {
    public class Health : MonoBehaviour {
        
        [SerializeField] private int maxHp = 100;
        [SerializeField] private int hp = 100;
        [SerializeField] private float stumbleDuration = 2f;
        [SerializeField] private int healthOnExitStumble = 40;
        [SerializeField] private float bloodOnKill = 10;
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Color stumbleColor = Color.red;
        private Color defaultColor;
        
        [SerializeField] private bool canStumble = true;
        private float stumbleTimeTracker;
        public bool Stumbled => stumbleTimeTracker >= 1e-4;

        private BloodTracker bloodTracker;
        private Abilities abilities;

        private void Start() {
            bloodTracker = FindObjectOfType<BloodTracker>();
            abilities = FindObjectOfType<Abilities>();
            defaultColor = spriteRenderer.color;
        }

        public void ApplyDamage(int damage) {
            hp -= damage;
            
            CheckOnHitEffects(false);
            gameObject.BroadcastMessage("OnDamage", SendMessageOptions.DontRequireReceiver);
        }

        public void ApplyMeleeDamage(int damage) {
            hp -= damage;

            CheckOnHitEffects(true);
            gameObject.BroadcastMessage("OnMeleeDamage", SendMessageOptions.DontRequireReceiver);
        }

        public void ApplyDamageForceKill(int damage) {
            hp -= damage;
            
            CheckDeath(true, abilities.BloodMultiplierForBloodRageKills);
            gameObject.BroadcastMessage("OnDamage", SendMessageOptions.DontRequireReceiver);
        }

        public void ApplyHeal(int heal) {
            hp += heal;
            if (hp > maxHp) {
                hp = maxHp;
            }
        }

        private void CheckOnHitEffects(bool killGivesBlood) {
            if (canStumble && !Stumbled && hp <= 1) {
                hp = 1;
                stumbleTimeTracker = stumbleDuration;
                spriteRenderer.color = stumbleColor;
            }else if (!canStumble || Stumbled) {
                CheckDeath(killGivesBlood);
            }
        }
        
        private void CheckDeath(bool killGivesBlood, float bloodMult = 1f) {
            if (hp <= 0)
                Die(killGivesBlood, bloodMult);
        }

        void Die(bool killGivesBlood, float bloodMult = 1f) {
            if (killGivesBlood) {
                bloodTracker.AddBlood(bloodOnKill * bloodMult);
            }

            gameObject.BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver); //Send to every object and call this function
        }

        void Update() {
            if (Stumbled) {
                stumbleTimeTracker -= Time.deltaTime;
                if (!Stumbled) { // Exiting stumble
                    ApplyHeal(healthOnExitStumble);
                    spriteRenderer.color = defaultColor;
                }
            }
        }
    }
}
