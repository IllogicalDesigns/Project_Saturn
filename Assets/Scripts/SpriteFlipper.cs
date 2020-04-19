using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    [SerializeField] float varience = 40f;
    [SerializeField] Sprite[] up;
    [SerializeField] Sprite[] dwn;

    Transform objToSnapTo;
    SpriteRenderer legR, legL, shirt, head;
    Transform trans;
    Quaternion rot;

    // Use this for initialization
    void Start() {
        trans = transform;
        objToSnapTo = trans.parent;
        shirt = trans.Find("Shirt").GetComponent<SpriteRenderer>();
        legR = trans.Find("Leg R").GetComponent<SpriteRenderer>();
        legL = trans.Find("Leg L").GetComponent<SpriteRenderer>();
        head = trans.Find("Head").GetComponent<SpriteRenderer>();
        rot = trans.rotation;
    }

    void HorizFlipUnderStander(Vector3 tar, Vector3 d) {
        float angle = Vector3.Angle(tar, d);
        float angle2 = Vector3.Angle(objToSnapTo.forward, d);

        if (shirt == null)
            this.enabled = false;

        if (angle2 <= varience) {
            shirt.flipX = true;
            head.flipX = true;
        }
        else {
            shirt.flipX = false;
            head.flipX = false;
        }
    }

    void VertFlipUnderStander(Vector3 tar, Vector3 d) {
        float angle = Vector3.Angle(tar, d);
        float angle2 = Vector3.Angle(objToSnapTo.right, d);

        if (angle2 <= varience) {
            head.sortingOrder = 0;
            head.sprite = up[0];
            shirt.sprite = up[1];
            legR.sprite = up[2];
            legL.sprite = up[3];
        }
        else {
            head.sortingOrder = 2;
            head.sprite = dwn[0];
            shirt.sprite = dwn[1];
            legR.sprite = dwn[2];
            legL.sprite = dwn[3];
        }
    }

    // Update is called once per frame
    void Update() {
        //trans.position = objToSnapTo.position;
        trans.rotation = rot;

        Vector3 _dir = (new Vector3(transform.position.x + Vector3.right.x, transform.position.y + Vector3.right.y, transform.position.z + Vector3.right.z) - transform.position);
        Vector3 targetDir = objToSnapTo.transform.forward - transform.position;
        HorizFlipUnderStander(targetDir, _dir);
        VertFlipUnderStander(targetDir, _dir);
    }
}
