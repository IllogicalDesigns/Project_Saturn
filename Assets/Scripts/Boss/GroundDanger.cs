using Player;
using UnityEngine;

namespace Boss {
    public class GroundDanger : MonoBehaviour {

        public float delay;
        public int damage;
        
        [SerializeField] private float damageRange;
        [SerializeField] private float effectDuration;
        
        [SerializeField] private GameObject sprite_indicator;
        [SerializeField] private GameObject sprite_effect;

        private Transform player;
        private Health playerHealth;
        private float timeToEffect;
        private float timeToExpire;
        private bool hasDoneEffect;

        private void Start() {
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            playerHealth = player.GetComponent<Health>();
            timeToEffect = delay;
        }

        private void Update() {
            if (timeToEffect > 1e-4) {
                timeToEffect -= Time.deltaTime;
            }
            else if (!hasDoneEffect){
                DoEffect();
            }else if (timeToExpire > 1e-4) {
                timeToExpire -= Time.deltaTime;
            }else if (hasDoneEffect) {
                Destroy(gameObject);
            }
        }

        private void DoEffect() {
            sprite_indicator.SetActive(false);
            sprite_effect.SetActive(true);
            timeToExpire = effectDuration;
            hasDoneEffect = true;

            if (Vector3.Distance(player.position, transform.position) > damageRange) return;
            
            playerHealth.ApplyDamage(damage);
        }

    }
}