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
        
    // Start is called before the first frame update
    void Start() {
        if (!gameObject.CompareTag("Player")) return;

        playerMovement = gameObject.GetComponent<Movement>();
        playerPistol = gameObject.GetComponent<Pistol>();
        playerMelee = gameObject.GetComponent<Melee>();
        playerAbilities = gameObject.GetComponent<Abilities>();
    }

    // Update is called once per frame
    void Update() { }

    public void OnDeath() {
        if (!gameObject.CompareTag("Player")) {
            var parent = transform.parent;
            Destroy(parent != null ? parent.gameObject : gameObject);
        }
        else {
            playerMovement.canMove = false;
            playerPistol.canFire = false;
            playerMelee.canAttack = false;
            playerAbilities.canUseAbilites = false;
        }
    }
}
