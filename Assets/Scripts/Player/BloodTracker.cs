using UnityEngine;

namespace Player {
    public class BloodTracker : MonoBehaviour {

        [SerializeField] public const float MaxBlood = 100f;
        [SerializeField] public const float BloodLossPerSecond = 1f;
        [SerializeField] public InGameCanvas canvas;
        private float currentBloodInt;
        public float CurrentBlood => currentBloodInt;

        public bool TryUseBlood(float amount) {
            if (amount > currentBloodInt) return false;

            currentBloodInt -= amount;
            return true;
        }
        
        void Start() {
            currentBloodInt = MaxBlood;
            canvas.SetupBloodSlider(MaxBlood, currentBloodInt);
        }

        void Update() {
            currentBloodInt -= BloodLossPerSecond * Time.deltaTime;
            canvas.UpdateBloodSlider(currentBloodInt);
        }

    }
}