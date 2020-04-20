using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class OnDeathScript : MonoBehaviour
{
    private Movement playerMovement;
    private Pistol playerPistol;
    private Melee playerMelee;
    private Abilities playerAbilities;
    [SerializeField] private AudioClip deathScream;
    [SerializeField] private AudioSource src;
    [SerializeField] private Animator animator;
        
    // Start is called before the first frame update
    void Start() {
        src = GetComponent<AudioSource>();

        if (!gameObject.CompareTag("Player")) return;

        playerMovement = gameObject.GetComponent<Movement>();
        playerPistol = gameObject.GetComponent<Pistol>();
        playerMelee = gameObject.GetComponent<Melee>();
        playerAbilities = gameObject.GetComponent<Abilities>();
    }

    // Update is called once per frame
    void Update() { }

    public void OnDeath() {
        if(animator != null) animator.SetTrigger("Die");

        if (src != null)
            src.PlayOneShot(deathScream);

        if (!gameObject.CompareTag("Player")) {
            var parent = transform.parent;
            //TODO play anim death
            Destroy(parent != null ? parent.gameObject : gameObject, 0.25f);
        }
        else {
            playerMovement.canMove = false;
            playerPistol.canFire = false;
            playerMelee.canAttack = false;
            playerAbilities.canUseAbilites = false;
            FindObjectOfType<InGameCanvas>().PlayerHasDied();
        }
    }
}
