using UnityEngine;

namespace Player {
    public class BloodTracker : MonoBehaviour {

        [SerializeField] public const float MaxBlood = 100f;
        [SerializeField] public const float BloodLossPerSecond = 2f;
        [SerializeField] public InGameCanvas canvas;
        private float currentBloodInt;
        private float lowBloodThresh = 50f;
        private float lowBloodThreshAdd = 70f;

        public float CurrentBlood => currentBloodInt;

        public bool TryUseBlood(float amount) {
            if (amount > currentBloodInt) return false;

            currentBloodInt -= amount;
            return true;
        }

        public void AddBlood(float amount) {
            amount = IncreaseAddedBloodBasedOnRemaining(amount);
            currentBloodInt += amount;
            if (currentBloodInt > MaxBlood) {
                currentBloodInt = MaxBlood;
            }
        }

        private float LowerBloodLossBasedOnRemaining(float _loss) {
            if (currentBloodInt < lowBloodThresh) {
                if (currentBloodInt < lowBloodThresh * 0.5f)
                    _loss = _loss * 0.25f;
                else
                    _loss = _loss * 0.5f;
            }

            return _loss;
        }

        private float IncreaseAddedBloodBasedOnRemaining(float _add) {
            if (currentBloodInt < lowBloodThreshAdd) {
                _add = _add * 3f * (lowBloodThreshAdd - currentBloodInt) / lowBloodThreshAdd;
            }

            return _add;
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