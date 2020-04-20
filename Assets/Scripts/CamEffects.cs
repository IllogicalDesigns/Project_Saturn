using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEffects : Singleton<CamEffects>
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	// How long the object should shake for.
	float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	bool frozen = false;

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

	IEnumerator frameFreeze() {
		float curTime = Time.timeScale;
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime(0.025f);
		Time.timeScale = curTime;
		frozen = false;
	}

	public void FreezeFrame() {
		if (frozen)
			return;

		frozen = true;
		StartCoroutine(frameFreeze());
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
