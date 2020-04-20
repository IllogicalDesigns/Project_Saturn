using System;
using UnityEngine;

namespace DefaultNamespace {
    public class UberBossButton : MonoBehaviour {
        private void Start() {
            if (PlayerPrefs.HasKey("UnlockedUberBosses")) {
                gameObject.SetActive(true);
            }
            else {
                gameObject.SetActive(false);
            }
        }
    }
}