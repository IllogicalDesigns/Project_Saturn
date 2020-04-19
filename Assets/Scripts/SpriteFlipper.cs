﻿using System.Collections;
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

    bool stumbling = false;
    float rateOfStumble = 0.1f, stumbleTimer;
    Color origClr;

    // Use this for initialization
    void Start() {
        trans = transform;
        objToSnapTo = trans.parent;
        shirt = trans.Find("Shirt").GetComponent<SpriteRenderer>();
        legR = trans.Find("Leg R").GetComponent<SpriteRenderer>();
        legL = trans.Find("Leg L").GetComponent<SpriteRenderer>();
        head = trans.Find("Head").GetComponent<SpriteRenderer>();
        rot = trans.rotation;

        origClr = shirt.color;
    }

    public void SetStumble(bool _stumbling) {
        stumbling = _stumbling;
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

    void stumbler() {
        stumbleTimer -= Time.deltaTime;

        if (stumbleTimer <= 0) {
            SwapStumble();
            stumbleTimer = rateOfStumble;
        }
    }

    void SwapStumble() {
        if(shirt.color == origClr) {
            head.color = Color.red;
            shirt.color = Color.red;
            legR.color = Color.red;
            legL.color = Color.red;
        }
        else {
            head.color = origClr;
            shirt.color = origClr;
            legR.color = origClr;
            legL.color = origClr;
        }
    }

    IEnumerator flashOnce() {
        head.color = Color.red;
        shirt.color = Color.red;
        legR.color = Color.red;
        legL.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        head.color = origClr;
        shirt.color = origClr;
        legR.color = origClr;
        legL.color = origClr;
    }

    public void OnDamage() {
        StartCoroutine(flashOnce());
    }

    // Update is called once per frame
    void Update() {
        //trans.position = objToSnapTo.position;
        trans.rotation = rot;

        Vector3 _dir = (new Vector3(transform.position.x + Vector3.right.x, transform.position.y + Vector3.right.y, transform.position.z + Vector3.right.z) - transform.position);
        Vector3 targetDir = objToSnapTo.transform.forward - transform.position;
        HorizFlipUnderStander(targetDir, _dir);
        VertFlipUnderStander(targetDir, _dir);

        if (stumbling) {
            stumbler();
        }
    }
}
