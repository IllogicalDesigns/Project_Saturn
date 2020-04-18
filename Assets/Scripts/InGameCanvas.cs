using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InGameCanvas : MonoBehaviour
{
    [SerializeField] private Slider dodgeSlider;
    [SerializeField] private Slider bloodSlider;

    public void SetupDodgeSlider(float _maxValue, float _value)
    {
        dodgeSlider.maxValue = _maxValue;
        dodgeSlider.value = _value;
    }

    public void UpdateDodgeSlider(float _value)
    {
        dodgeSlider.value = _value;
    }
    
    public void SetupBloodSlider(float _maxValue, float _value)
    {
        bloodSlider.maxValue = _maxValue;
        bloodSlider.value = _value;
    }

    public void UpdateBloodSlider(float _value)
    {
        bloodSlider.value = _value;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
