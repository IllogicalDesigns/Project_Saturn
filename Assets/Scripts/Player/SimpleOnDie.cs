using UnityEngine;

namespace Player {
    public class SimpleOnDie : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start() {
        
        }

        // Update is called once per frame
        void Update() {
        
        }

        public void OnDeath() {
            Destroy(gameObject);
        }
    }
}
