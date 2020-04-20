using System.Collections.Generic;
using Player;
using UnityEngine;

namespace DefaultNamespace {
    public class SwordMessageOnBloodKill : MonoBehaviour {

        [SerializeField] private List<string> messages;
        [SerializeField] private float duration;

        private void OnBloodKill() {
            var message = new SwordMessage{
                Message = messages[Random.Range(0, messages.Count)],
                Duration = duration
            };
            GameObject.FindWithTag("SwordSpeech").SendMessage("OnSwordMessage", message);
        }

    }
}