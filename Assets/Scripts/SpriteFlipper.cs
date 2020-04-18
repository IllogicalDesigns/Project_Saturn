using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    [SerializeField] Transform objToSnapTo;
    Transform trans;
    [SerializeField] float varience = 40f;
    SpriteRenderer sR;
    [SerializeField] Sprite up;
    [SerializeField] Sprite dwn;

    // Use this for initialization
    void Start() {
        trans = transform;
        sR = gameObject.GetComponent<SpriteRenderer>();
    }

    void HorizFlipUnderStander(Vector3 tar, Vector3 d) {
        float angle = Vector3.Angle(tar, d);
        float angle2 = Vector3.Angle(objToSnapTo.forward, d);

        if (angle2 <= varience) {
            sR.flipX = true;
        }
        else {
            sR.flipX = false;
        }
    }

    void VertFlipUnderStander(Vector3 tar, Vector3 d) {
        float angle = Vector3.Angle(tar, d);
        float angle2 = Vector3.Angle(objToSnapTo.right, d);

        if (angle2 <= varience) {
            sR.sprite = up;
        }
        else {
            sR.sprite = dwn;
        }
    }

    // Update is called once per frame
    void Update() {
        trans.position = objToSnapTo.position;

        Vector3 _dir = (new Vector3(transform.position.x + Vector3.right.x, transform.position.y + Vector3.right.y, transform.position.z + Vector3.right.z) - transform.position);
        Vector3 targetDir = objToSnapTo.transform.forward - transform.position;
        HorizFlipUnderStander(targetDir, _dir);
        VertFlipUnderStander(targetDir, _dir);
    }
}
