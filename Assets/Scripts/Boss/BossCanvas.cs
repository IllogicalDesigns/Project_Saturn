using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Boss {
    public class BossCanvas : MonoBehaviour {

        [SerializeField] private Slider healthSlider;
        [SerializeField] private Health bossHealth;

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() {
            healthSlider.value = bossHealth.PercentHp;
        }
    }
}
