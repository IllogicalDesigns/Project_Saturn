using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    [SerializeField] private string tagToDamage = "Player";
    [SerializeField] private int damage = 25;
    [SerializeField] private bool destroyOnTrigger = true;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(tagToDamage)) {
            other.SendMessage("ApplyDamage", damage);
        }

        if(destroyOnTrigger)
            Destroy(gameObject);
    }
}
