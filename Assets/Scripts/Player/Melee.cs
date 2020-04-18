using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private bool canPunch;
    private string fire;

    public float timeInBetweenPunches = 1f;
    private float punchTimer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (punchTimer >= 0)
            punchTimer -= Time.deltaTime;

        if (Input.GetButtonDown(fire) && canPunch && punchTimer <= 0) {
            //punchDamnIt();

            punchTimer = timeInBetweenPunches;
        }
    }
}
