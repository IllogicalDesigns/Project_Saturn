using UnityEngine;

namespace Player {
    public class Abilities : MonoBehaviour {

        [SerializeField] private BloodTracker bloodTracker;
        [SerializeField] private Movement playerMovement;
        [SerializeField] private Health playerHealth;
        
        [SerializeField] private float bloodUsePerHeal;
        [SerializeField] private int healAmount;
        [SerializeField] private float healCooldown;
        private float healCooldownTracker;

        [SerializeField] private float bloodUsePerSecondTimeWarp;
        [SerializeField] private float minBloodForTimeWarp;
        [SerializeField] private float timeWarpFactor;
        private bool inTimeWarp;
        
        [SerializeField] private float bloodUsePerBloodRage;
        [SerializeField] private float bloodRageCooldown;
        private float bloodRageCooldownTracker;

        private float defaultFixedDeltaTime;

        private void Start() {
            defaultFixedDeltaTime = Time.fixedDeltaTime;
        }
        
        private void Update() {

            // Update cooldowns
            if (healCooldownTracker > 0) {
                healCooldownTracker -= Time.time;
            }
            if (bloodRageCooldownTracker > 0) {
                bloodRageCooldownTracker -= Time.time;
            }
            
            // Do timewarp maintain if in timewarp
            if (inTimeWarp) {
                MaintainTimeWarp();
            }
            
            if (Input.GetButtonDown("Heal") && healCooldownTracker <= 1e-4) {
                DoHeal();
            }else if (Input.GetButtonDown("TimeWarp") && !inTimeWarp) {
                StartTimeWarp();
            }else if (Input.GetButtonDown("TimeWarp") && inTimeWarp) {
                EndTimeWarp();
            }
            else if (Input.GetButtonDown("BloodRage") && bloodRageCooldownTracker <= 1e-4) {
                
            }
            
        }

        private void DoHeal() {
            if (!bloodTracker.TryUseBlood(bloodUsePerHeal)) return;
            
            playerHealth.ApplyHeal(healAmount);
            healCooldownTracker = healCooldown;
        }

        private void StartTimeWarp() {
            if (bloodTracker.CurrentBlood < minBloodForTimeWarp) return;

            inTimeWarp = true;
            Time.timeScale *= timeWarpFactor;
            Time.fixedDeltaTime *= timeWarpFactor;
            playerMovement.MovementFactor = 1 / timeWarpFactor;
        }

        private void MaintainTimeWarp() {
            if (bloodTracker.CurrentBlood <= minBloodForTimeWarp) {
                EndTimeWarp();
                return;
            }

            bloodTracker.TryUseBlood(bloodUsePerSecondTimeWarp * Time.deltaTime);
        }

        private void EndTimeWarp() {
            inTimeWarp = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = defaultFixedDeltaTime;
            playerMovement.MovementFactor = 1f;
        }

    }
}