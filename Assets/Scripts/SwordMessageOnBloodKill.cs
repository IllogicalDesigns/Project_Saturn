using System.Collections.Generic;
using Player;
using UnityEngine;

namespace DefaultNamespace {
    public class SwordMessageOnBloodKill : MonoBehaviour {

        [SerializeField] private List<string> messages;
        [SerializeField] private float duration;
        [SerializeField] private float messageBaseChance = 0.3f;

        private void OnBloodKill() {
            var chance = messageBaseChance * FindObjectOfType<BloodTracker>().GetMessageChanceAdjustment();
            if (Random.value > chance) return;
            
            var message = new SwordMessage{
                Message = messages[Random.Range(0, messages.Count)],
                Duration = duration
            };
            GameObject.FindWithTag("SwordSpeech").SendMessage("OnSwordMessage", message);
        }

    }
}