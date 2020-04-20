using UnityEngine;

namespace Player {
    public class BloodTracker : MonoBehaviour {

        [SerializeField] public const float MaxBlood = 100f;
        [SerializeField] public const float BloodLossPerSecond = 2f;
        [SerializeField] public InGameCanvas canvas;
        private float currentBloodInt;
        private float lowBloodThresh = 60f;
        private float lowBloodThreshAdd = 70f;
        private bool hasRunOutOfBloodThisLoad;
        
        private Movement playerMovement;
        private Pistol playerPistol;
        private Melee playerMelee;
        private Abilities playerAbilities;

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
                _loss = _loss * (1f - 0.7f * (lowBloodThresh - currentBloodInt) / lowBloodThresh);
            }

            return _loss;
        }

        private float IncreaseAddedBloodBasedOnRemaining(float _add) {
            if (currentBloodInt < lowBloodThreshAdd) {
                _add = _add * (1f + 2f * (lowBloodThreshAdd - currentBloodInt) / lowBloodThreshAdd);
            }

            return _add;
        }

        void Start() {
            currentBloodInt = MaxBlood;
            canvas.SetupBloodSlider(MaxBlood, currentBloodInt);

            var player = GameObject.FindWithTag("Player");
            playerMovement = player.GetComponent<Movement>();
            playerPistol = player.GetComponent<Pistol>();
            playerMelee = player.GetComponent<Melee>();
            playerAbilities = player.GetComponent<Abilities>();
        }

        void Update() {
            currentBloodInt -= LowerBloodLossBasedOnRemaining(BloodLossPerSecond) * Time.deltaTime;
            canvas.UpdateBloodSlider(currentBloodInt);

            if (!hasRunOutOfBloodThisLoad && CurrentBlood < 1e-4) {
                hasRunOutOfBloodThisLoad = true;
                playerMovement.canMove = false;
                playerPistol.canFire = false;
                playerMelee.canAttack = false;
                playerAbilities.canUseAbilites = false;
                canvas.PlayerHasNoBlood();
            }
        }

    }
}