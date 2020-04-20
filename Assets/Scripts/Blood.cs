using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : Singleton<Blood> {
    [SerializeField] private ParticleSystem bloodySystem, wallSystem;
    // (Optional) Prevent non-singleton constructor use.
    protected Blood() { }

    public void EmitBlood(Transform _transform) {
        transform.rotation = _transform.rotation;
        transform.position = _transform.position;
        bloodySystem.Emit(30);
    }

    public void EmitWallImpact(Transform _transform) {
        transform.rotation = _transform.rotation;
        transform.Rotate(Vector3.up, 180f);
        transform.position = _transform.position;
        wallSystem.Emit(5);
    }
}
