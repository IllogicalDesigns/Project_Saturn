using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEffects : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	// How long the object should shake for.
	float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void Awake() {
		if (camTransform == null) {
			camTransform = transform;
		}
	}

	void OnEnable() {
		originalPos = camTransform.localPosition;
	}

	public void Shake(float _shakeDuration) {
		shakeDuration = _shakeDuration;
	}

	public void Shake(float _shakeDuration, float _shakeAmount) {
		shakeDuration = _shakeDuration;
		shakeAmount = _shakeAmount;
	}

	void Update() {
		if (shakeDuration > 0) {
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else {
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
		}
	}
}
