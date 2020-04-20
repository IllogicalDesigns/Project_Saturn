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
    [SerializeField] private GameObject DeadPanel;
    [SerializeField] private GameObject DeadText;
    [SerializeField] private GameObject BloodText;
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

    public void PlayerHasDied() {
        transform.parent.BroadcastMessage("onDisablePlayer", SendMessageOptions.DontRequireReceiver);
        CursorLock(false);
        DeadPanel.SetActive(true);
        DeadText.SetActive(true);
        BloodText.SetActive(false);
        Time.timeScale = 0.001f;
    }

    public void PlayerHasNoBlood() {
        transform.parent.BroadcastMessage("onDisablePlayer", SendMessageOptions.DontRequireReceiver);
        CursorLock(false);
        DeadPanel.SetActive(true);
        DeadText.SetActive(false);
        BloodText.SetActive(true);
        Time.timeScale = 0.001f;
    }

    public void TogglePauseApp() {
        if (!PausePanel.activeInHierarchy) {
            transform.parent.BroadcastMessage("onDisablePlayer", SendMessageOptions.DontRequireReceiver);
            CursorLock(false);
            PausePanel.SetActive(true);
            Time.timeScale = 0.001f;
        }
        else {
            transform.parent.BroadcastMessage("onEnablePlayer", SendMessageOptions.DontRequireReceiver);
            CursorLock(true);
            PausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private static void CursorLock(bool locked) {
        if(locked)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.None;
        Cursor.visible = !locked;
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