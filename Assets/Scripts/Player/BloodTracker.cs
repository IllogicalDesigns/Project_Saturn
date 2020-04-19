using UnityEngine;

namespace Player {
    public class BloodTracker : MonoBehaviour {

        [SerializeField] public const float MaxBlood = 100f;
        [SerializeField] public const float BloodLossPerSecond = 2f;
        [SerializeField] public InGameCanvas canvas;
        private float currentBloodInt;
        private int lowHpDmgThresh = 50;

        public float CurrentBlood => currentBloodInt;

        public bool TryUseBlood(float amount) {
            if (amount > currentBloodInt) return false;

            currentBloodInt -= amount;
            return true;
        }

        public void AddBlood(float amount) {
            currentBloodInt += amount;
            if (currentBloodInt > MaxBlood) {
                currentBloodInt = MaxBlood;
            }
        }

        private float LowerBloodLossBasedOnRemaining(float _loss) {
            if (CompareTag("Player") && currentBloodInt < lowHpDmgThresh) {
                if (currentBloodInt < lowHpDmgThresh * 0.5f)
                    _loss = _loss * 0.25f;
                else
                    _loss = _loss * 0.5f;
            }

            return _loss;
        }

        void Start() {
            currentBloodInt = MaxBlood;
            canvas.SetupBloodSlider(MaxBlood, currentBloodInt);
        }

        void Update() {
            currentBloodInt -= LowerBloodLossBasedOnRemaining(BloodLossPerSecond) * Time.deltaTime;
            canvas.UpdateBloodSlider(currentBloodInt);
        }

    }
}