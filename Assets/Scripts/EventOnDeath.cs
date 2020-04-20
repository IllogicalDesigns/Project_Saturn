using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnDeath : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] UnityEvent DelayOnDeath;
    [SerializeField] UnityEvent onDeath;

    IEnumerator waitBeforeTrigger() {
        yield return new WaitForSeconds(delay);
        DelayOnDeath?.Invoke();
    }

    public void OnDeath() {
        onDeath?.Invoke();
        StartCoroutine(waitBeforeTrigger());
    }
}
