using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DianaFlipper : SpriteFlipper {
    [SerializeField] private Animator anim;
    bool faceUp = false, faceRight = false;
    [SerializeField] Transform sprite;


    void Start()
    {
        objToSnapTo = this.transform;
    }

    public virtual void SetStumble(bool _stumbling) {
        stumbling = _stumbling;
    }

    void HorizFlipUnderStander(Vector3 tar, Vector3 d) {
        float angle = Vector3.Angle(tar, d);
        float angle2 = Vector3.Angle(objToSnapTo.forward, d);

        /*if (shirt == null)
            this.enabled = false;*/

        if (angle2 <= varience) {
            /*shirt.flipX = true;
            head.flipX = true;*/
            faceRight = true;
        }
        else {
            /*shirt.flipX = false;
            head.flipX = false;*/
            faceRight = false;
        }
    }

   /* void stumbler() {
        stumbleTimer -= Time.deltaTime;

        if (stumbleTimer <= 0) {
            SwapStumble();
            stumbleTimer = rateOfStumble;
        }
    }

    void SwapStumble() {
        *//*if (shirt.color == origClr) {
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
        }*//*
    }*/

    void VertFlipUnderStander(Vector3 tar, Vector3 d) {
        float angle = Vector3.Angle(tar, d);
        float angle2 = Vector3.Angle(objToSnapTo.right, d);

        if (angle2 <= varience) {
            /* head.sortingOrder = 0;
             head.sprite = up[0];
             shirt.sprite = up[1];
             legR.sprite = up[2];
             legL.sprite = up[3];*/
            faceUp = true;
        }
        else {
            /*head.sortingOrder = 2;
            head.sprite = dwn[0];
            shirt.sprite = dwn[1];
            legR.sprite = dwn[2];
            legL.sprite = dwn[3];*/
            faceUp = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        sprite.rotation = Quaternion.identity;
        Vector3 _dir = (new Vector3(transform.position.x + Vector3.right.x, transform.position.y + Vector3.right.y, transform.position.z + Vector3.right.z) - transform.position);
        Vector3 targetDir = objToSnapTo.transform.forward - transform.position;
        HorizFlipUnderStander(targetDir, _dir);
        VertFlipUnderStander(targetDir, _dir);

        if (stumbling) {
            //stumbler();
            if (!anim.GetBool("Stumbling")) 
                anim.SetBool("Stumbling", true);
        }
        else {
            if (anim.GetBool("Stumbling")) 
                anim.SetBool("Stumbling", false);
        }

        if (faceRight)
            anim.SetFloat("LeftRight", 1);
        else
            anim.SetFloat("LeftRight", -1);

        if (faceUp)
            anim.SetFloat("UpDown", 1);
        else
            anim.SetFloat("UpDown", -1);
    }
}
