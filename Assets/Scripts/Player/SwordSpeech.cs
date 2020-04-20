using TMPro;
using UnityEngine;

namespace Player {
    public class SwordSpeech : MonoBehaviour {

        [SerializeField] private GameObject speech;
        [SerializeField] private TextMeshProUGUI text;

        private float durationTracker;
        
        private void OnSwordMessage(SwordMessage message) {
            speech.SetActive(true);
            text.text = message.Message;
            durationTracker = message.Duration;
        }

        private void Update() {
            if (!speech.activeSelf) return;
            
            if (durationTracker > 1e-4) {
                durationTracker -= Time.deltaTime;
            }
            else {
                speech.SetActive(false);
            }
        }
        
    }

    public struct SwordMessage {
        public string Message;
        public float Duration;
    }
}