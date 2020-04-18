using UnityEngine.UI;
using UnityEngine;

public class InGameCanvas : MonoBehaviour {
    [SerializeField] private Slider dodgeSlider;
    [SerializeField] private Slider bloodSlider;
    [SerializeField] private GameObject timeWarpOverlay;
    [SerializeField] private GameObject bloodRageOverlay;

    public void SetupDodgeSlider(float _maxValue, float _value) {
        dodgeSlider.maxValue = _maxValue;
        dodgeSlider.value = _value;
    }

    public void UpdateDodgeSlider(float _value) {
        dodgeSlider.value = _value;
    }

    public void SetupBloodSlider(float _maxValue, float _value) {
        bloodSlider.maxValue = _maxValue;
        bloodSlider.value = _value;
    }

    public void UpdateBloodSlider(float _value) {
        bloodSlider.value = _value;
    }

    public void EnableTimeWarpOverlay() {
        timeWarpOverlay.SetActive(true);
    }

    public void DisableTimeWarpOverlay() {
        timeWarpOverlay.SetActive(false);
    }
    
    public void EnableBloodRageOverlay() {
        bloodRageOverlay.SetActive(true);
    }

    public void DisableBloodRageOverlay() {
        bloodRageOverlay.SetActive(false);
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}