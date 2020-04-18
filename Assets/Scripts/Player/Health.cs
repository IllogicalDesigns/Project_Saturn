using UnityEngine;

namespace Player {
    public class Health : MonoBehaviour {
        
        public int maxHp = 100;
        public int hp = 100;

        public void ApplyDamage(int damage) {
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

        void CheckDeath() {
            if (hp <= 0)
                Die();
        }

        void Die() {
            gameObject.BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver); //Send to every object and call this function
        }
    }
}
