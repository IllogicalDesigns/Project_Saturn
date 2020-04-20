using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventOnTrigger : MonoBehaviour
{
    public UnityEvent onTrigger;


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            onTrigger.Invoke();
    }
}
