using UnityEngine;

namespace Player {
    public class Abilities : MonoBehaviour {

        [SerializeField] private BloodTracker bloodTracker;
        [SerializeField] private Movement playerMovement;
        [SerializeField] private Health playerHealth;
        [SerializeField] private InGameCanvas canvas;
        
        [SerializeField] private float bloodUsePerHeal;
        [SerializeField] private int healAmount;
        [SerializeField] private float healCooldown;
        private float healCooldownTracker;

        [SerializeField] private float bloodUsePerSecondTimeWarp;
        [SerializeField] private float minBloodForTimeWarp;
        [SerializeField] private float timeWarpFactor;
        private bool inTimeWarp;
        
        [SerializeField] private float bloodUsePerBloodRage;
        [SerializeField] private int chargeDamageInBloodRage;
        [SerializeField] public float BloodMultiplierForBloodRageKills;
        [SerializeField] private float bloodRageDuration;
        [SerializeField] private float bloodRageCooldown;
        private float bloodRageTimeTracker;
        private float bloodRageCooldownTracker;

        private float defaultFixedDeltaTime;

        public bool canUseAbilites = true;

        private void Start() {
            defaultFixedDeltaTime = Time.fixedDeltaTime;
        }
        
        private void Update() {

            // Update cooldowns
            if (healCooldownTracker > 0) {
                healCooldownTracker -= Time.deltaTime;
            }
            if (bloodRageTimeTracker <= 1e-4 && bloodRageCooldownTracker > 0) {
                bloodRageCooldownTracker -= Time.deltaTime;
            }

            // Do timewarp maintain if in timewarp
            if (inTimeWarp) {
                MaintainTimeWarp();
            }
            
            // Tick bloodrage if in bloodrage
            if (bloodRageTimeTracker > 0) {
                bloodRageTimeTracker -= Time.deltaTime;
                if (bloodRageTimeTracker <= 1e-4) { // Exit blood rage
                    EndBloodRage();
                }
            }

            if (!canUseAbilites) return;
            
            // Try to get inputs
            if (Input.GetButtonDown("Heal") && healCooldownTracker <= 1e-4) {
                DoHeal();
            }else if (Input.GetButtonDown("TimeWarp") && !inTimeWarp) {
                StartTimeWarp();
            }else if (Input.GetButtonDown("TimeWarp") && inTimeWarp) {
                EndTimeWarp();
            }
            else if (Input.GetButtonDown("BloodRage") && bloodRageCooldownTracker <= 1e-4) {
                StartBloodRage();
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
            canvas.EnableTimeWarpOverlay();
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
            canvas.DisableTimeWarpOverlay();
            Time.timeScale = 1f;
            Time.fixedDeltaTime = defaultFixedDeltaTime;
            playerMovement.MovementFactor = 1f;
        }

        private void StartBloodRage() {
            if (!bloodTracker.TryUseBlood(bloodUsePerBloodRage)) return;

            canvas.EnableBloodRageOverlay();
            bloodRageTimeTracker = bloodRageDuration;
            bloodRageCooldownTracker = bloodRageCooldown;
        }

        private void EndBloodRage() {
            canvas.DisableBloodRageOverlay();
        }

        private void OnCollisionEnter(Collision other) {
            if (bloodRageTimeTracker <= 1e-4 || !other.gameObject.CompareTag("Enemy")) return;
            
            other.gameObject.SendMessage("ApplyDamageForceKill", chargeDamageInBloodRage);
        }
    }
}