using UnityEngine;

namespace Player {
    public class Health : MonoBehaviour
    {
        public int hp = 100;

        public void ApplyDamage(int damage) {
            hp -= damage;
            CheckDeath();
            gameObject.BroadcastMessage("OnDamage", SendMessageOptions.DontRequireReceiver);
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
