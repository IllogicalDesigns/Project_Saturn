using System.Collections;
using UnityEngine;

namespace Player {
    public class Melee : MonoBehaviour
    {
        float attackTime = 0.2f;
        bool canAttack = true;
        [SerializeField] GameObject meleeCollider;


        IEnumerator Punch() {
            canAttack = false;
            meleeCollider.SetActive(true);
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
