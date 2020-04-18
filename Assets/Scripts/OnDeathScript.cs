using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathScript : MonoBehaviour
{
    public void OnDeath() {
        Destroy(transform.parent.gameObject);
    }
}
