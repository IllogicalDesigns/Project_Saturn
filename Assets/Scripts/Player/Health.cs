using System;
using UnityEngine;

namespace Player {
    public class Health : MonoBehaviour {
        
        [SerializeField] private int maxHp = 100;
        [SerializeField] private int hp = 100;
        [SerializeField] private float stumbleDuration = 2f;
        [SerializeField] private int healthOnExitStumble = 40;
        [SerializeField] private float bloodOnKill = 25;
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Color stumbleColor = Color.red;
        private Color defaultColor;
        
        [SerializeField] private bool canStumble = true;
        [SerializeField] private bool canDieToBulletInStuble = true;
        [SerializeField] SpriteFlipper spriteFlipper;
        private float stumbleTimeTracker;
        public bool Stumbled => stumbleTimeTracker >= 1e-4;

        public int Hp => hp;
        public float PercentHp => (float)hp / maxHp;

        int lowHpDmgThresh = 50;
        
        private BloodTracker bloodTracker;
        private Abilities abilities;

        private void Start() {
            bloodTracker = FindObjectOfType<BloodTracker>();
            abilities = FindObjectOfType<Abilities>();
        }

        public void ApplyDamage(int damage) {
            damage = LowerDamageBasedOnHp(damage);

            hp -= damage;
            
            CheckOnHitEffects(false, true);
            gameObject.BroadcastMessage("OnDamage", SendMessageOptions.DontRequireReceiver);
        }

        public void ApplyMeleeDamage(int damage) {
            damage = LowerDamageBasedOnHp(damage);

            hp -= damage;

            CheckOnHitEffects(true, false);
            gameObject.BroadcastMessage("OnMeleeDamage", SendMessageOptions.DontRequireReceiver);
        }

        private int LowerDamageBasedOnHp(int damage) {
            if (this.CompareTag("Player") && hp < lowHpDmgThresh) {
                if (hp < lowHpDmgThresh * 0.5f)
                    damage = Mathf.RoundToInt(damage * 0.25f);
                else
                    damage = Mathf.RoundToInt(damage * 0.5f);
            }

            return damage;
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

        private void CheckOnHitEffects(bool killGivesBlood, bool isBullet) {
            if (canStumble && !Stumbled && hp <= 1) {
                hp = 1;
                stumbleTimeTracker = stumbleDuration;
                spriteFlipper.SetStumble(true);
                gameObject.BroadcastMessage("EnteredStumble", SendMessageOptions.DontRequireReceiver);
            }else if ((!canStumble || Stumbled) && (canDieToBulletInStuble || !isBullet)) {
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
                    spriteFlipper.SetStumble(false);
                    gameObject.BroadcastMessage("ExitedStumble", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
