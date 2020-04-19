using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : Singleton<Blood> {
    [SerializeField] private ParticleSystem particleSystem;
    // (Optional) Prevent non-singleton constructor use.
    protected Blood() { }

    public void EmitBlood(Transform _transform) {
        transform.rotation = _transform.rotation;
        transform.position = _transform.position;
        particleSystem.Emit(30);
    }
}
