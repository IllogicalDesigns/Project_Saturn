using System.Collections;
using UnityEngine;

namespace Player {
    public class Melee : MonoBehaviour
    {
        float attackTime = 0.2f;
        public bool canAttack = true;
        [SerializeField] GameObject meleeCollider;
        [SerializeField] AudioClip swing;
        [SerializeField] AudioSource source;

        IEnumerator Punch() {
            canAttack = false;
            meleeCollider.SetActive(true);
            source.PlayOneShot(swing);
            yield return new WaitForSeconds(attackTime);
            meleeCollider.SetActive(false);
            canAttack = true;
        }

        public void Update() {
            if (Input.GetButtonDown("Fire2") && canAttack) {
                StartCoroutine(Punch());
            }
        }
    }
}
