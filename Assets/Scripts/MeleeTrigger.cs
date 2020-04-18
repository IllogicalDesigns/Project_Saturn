using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeleeTrigger : MonoBehaviour {
    [SerializeField] string hitAbleTag =  "Player";
    [SerializeField] int dam = 35;
    [SerializeField] int knockForce = 35;
    [SerializeField] CamEffects camEffects;
    //[SerializeField] UnityEvent OnHit;
    float shakeDir = 0.1f, shakeAmt = 0.5f;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(hitAbleTag)) {
            other.SendMessage("ApplyMeleeDamage", dam);  //Damage it
            other.SendMessage("ApplyKnockback", new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, knockForce), SendMessageOptions.DontRequireReceiver);  //apply kickback for this weapon
            if (camEffects != null) {
                camEffects.Shake(shakeDir, shakeAmt);
            }
        }
    }
}
