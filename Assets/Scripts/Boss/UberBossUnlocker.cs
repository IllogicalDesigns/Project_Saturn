using UnityEngine;

namespace Boss {
    public class UberBossUnlocker : MonoBehaviour {

        private void OnDeath() {
            PlayerPrefs.SetInt("UnlockedUberBosses", 1);
            PlayerPrefs.Save();
        }
        
    }
}