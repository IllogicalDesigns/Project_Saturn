using System;
using UnityEngine;

namespace Player {
    public class Health : MonoBehaviour {
        
        [SerializeField] private int maxHp = 100;
        [SerializeField] private int hp = 100;
        [SerializeField] private float stumbleDuration = 2f;
        [SerializeField] private int healthOnExitStumble = 40;
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Color stumbleColor = Color.red;
        private Color defaultColor;
        
        private bool canStumble;
        private float stumbleTimeTracker;
        private bool Stumbled => stumbleTimeTracker >= 1e-4;

        private void Start() {
            canStumble = gameObject.CompareTag("Enemy");
            defaultColor = spriteRenderer.color;
        }

        public void ApplyDamage(int damage) {
            hp -= damage;
            
            CheckOnHitEffects();
            gameObject.BroadcastMessage("OnDamage", SendMessageOptions.DontRequireReceiver);
        }

        public void ApplyDamageForceKill(int damage) {
            hp -= damage;
            
            CheckDeath();
            gameObject.BroadcastMessage("OnDamage", SendMessageOptions.DontRequireReceiver);
        }

        public void ApplyHeal(int heal) {
            hp += heal;
            if (hp > maxHp) {
                hp = maxHp;
            }
        }

        private void CheckOnHitEffects() {
            if (canStumble && !Stumbled && hp <= 1) {
                hp = 1;
                stumbleTimeTracker = stumbleDuration;
                spriteRenderer.color = stumbleColor;
            }else if (!canStumble || Stumbled) {
                CheckDeath();
            }
        }
        
        private void CheckDeath() {
            if (hp <= 0)
                Die();
        }

        void Die() {
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
