using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using Player;

public class InGameCanvas : MonoBehaviour {
    [SerializeField] private Slider dodgeSlider;
    [SerializeField] private Slider bloodSlider;
    [SerializeField] private Slider HpSlider;
    [SerializeField] private GameObject timeWarpOverlay;
    [SerializeField] private GameObject bloodRageOverlay;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Health playerHealth;

    public void SetupDodgeSlider(float _maxValue, float _value) {
        dodgeSlider.maxValue = _maxValue;
        dodgeSlider.value = _value;
        HideDodgeIfFull();
    }

    public void UpdateDodgeSlider(float _value) {
        dodgeSlider.value = _value;
        HideDodgeIfFull();
    }

    private void HideDodgeIfFull() {
        if (dodgeSlider.value == dodgeSlider.maxValue)
            dodgeSlider.gameObject.SetActive(false);
        else {
            dodgeSlider.gameObject.SetActive(true);
        }
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

    public void TogglePauseApp() {
        if (!PausePanel.activeInHierarchy) {
            BroadcastMessage("onDisablePlayer", SendMessageOptions.DontRequireReceiver);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PausePanel.SetActive(true);
            Time.timeScale = 0.001f;
        }
        else {
            BroadcastMessage("onEnablePlayer", SendMessageOptions.DontRequireReceiver);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            PausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void SetLevel(float sliderValue) {
        mixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Volume", sliderValue);
    }

    // Start is called before the first frame update
    void Start() {
        SetLevel(PlayerPrefs.GetFloat("Volume", 0));
    }

    // Update is called once per frame
    void Update() { 
        if(Input.GetKeyDown(KeyCode.Escape)) {
            TogglePauseApp();
        }

        HpSlider.value = playerHealth.Hp;
    }
}