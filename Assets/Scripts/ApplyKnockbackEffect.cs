using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyKnockbackEffect : MonoBehaviour
{
    float defaultForce = 25f;
    public void ApplyKnockback(Vector4 pos) {
        var posFrom = new Vector3(pos.x, pos.y, pos.z);
        var force = pos.w;
        var dir = (posFrom - transform.position).normalized;
        var ridge = GetComponent<Rigidbody>();
        ridge.AddForce(-dir * force, ForceMode.Impulse);
    }

    public void ApplyKnockbackVec3(Vector3 pos) {
        var posFrom = new Vector3(pos.x, pos.y, pos.z);
        var dir = (posFrom - transform.position).normalized;
        var ridge = GetComponent<Rigidbody>();
        ridge.AddForce(-dir * defaultForce, ForceMode.Impulse);
    }

}
