using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    public float varience = 40f;
    [SerializeField] private Sprite[] up;
    [SerializeField] private Sprite[] dwn;

    public Transform objToSnapTo;
    private SpriteRenderer legR, legL, shirt, head;
    private Transform trans;
    private Quaternion rot;

    public bool stumbling = false;
    public float rateOfStumble = 0.1f, stumbleTimer;
    private bool invincible = false;
    private float invTimer;
    private Color stumbleColor = new Color(255f/255f, 204f/255f, 51f/255f);
    private Color invFrameColor = new Color(69f/255f, 255f/255f, 255f/255f);
    private Color origClr;

    [SerializeField] Animator anim;

    // Use this for initialization
    private void Start() {
        trans = transform;
        objToSnapTo = trans.parent;
        shirt = trans.Find("Shirt").GetComponent<SpriteRenderer>();
        legR = trans.Find("Leg R").GetComponent<SpriteRenderer>();
        legL = trans.Find("Leg L").GetComponent<SpriteRenderer>();
        head = trans.Find("Head").GetComponent<SpriteRenderer>();
        rot = trans.rotation;

        origClr = shirt.color;
    }

    public virtual void EnteredStumble() {
        stumbling = true;
    }

    public virtual void ExitedStumble() {
        stumbling = false;
        head.color = origClr;
        shirt.color = origClr;
        legR.color = origClr;
        legL.color = origClr;
    }

    public void DoInvFrames(float duration) {
        invincible = true;
        invTimer = duration;
        SwapInvincible();
    }

    private void HorizFlipUnderStander(Vector3 tar, Vector3 d) {
        float angle = Vector3.Angle(tar, d);
        float angle2 = Vector3.Angle(objToSnapTo.forward, d);

        if (shirt == null)
            enabled = false;

        if (angle2 <= varience) {
            shirt.flipX = true;
            head.flipX = true;
        }
        else {
            shirt.flipX = false;
            head.flipX = false;
        }
    }

    private void VertFlipUnderStander(Vector3 tar, Vector3 d) {
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

    private void Stumbler() {
        stumbleTimer -= Time.deltaTime;

        if (stumbleTimer <= 0) {
            SwapStumble();
            stumbleTimer = rateOfStumble;
        }
    }

    private void SwapStumble() {
        if(shirt.color == origClr) {
            head.color = stumbleColor;
            shirt.color = stumbleColor;
            legR.color = stumbleColor;
            legL.color = stumbleColor;
        }
        else {
            head.color = origClr;
            shirt.color = origClr;
            legR.color = origClr;
            legL.color = origClr;
        }
    }

    private void Invincibler() {
        if (invTimer > 1e-4) {
            invTimer -= Time.deltaTime;
        }
        else {
            head.color = origClr;
            shirt.color = origClr;
            legR.color = origClr;
            legL.color = origClr;
            invincible = false;
        }
    }

    private void SwapInvincible() {
        if(shirt.color == origClr) {
            head.color = invFrameColor;
            shirt.color = invFrameColor;
            legR.color = invFrameColor;
            legL.color = invFrameColor;
        }
        else {
            head.color = origClr;
            shirt.color = origClr;
            legR.color = origClr;
            legL.color = origClr;
        }
    }

    private IEnumerator flashOnce() {
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

    public void OnMeleeDamage() {
        StartCoroutine(flashOnce());
    }

    // Update is called once per frame
    private void Update() {
        //trans.position = objToSnapTo.position;
        trans.rotation = rot;

        Vector3 dir = (new Vector3(transform.position.x + Vector3.right.x, transform.position.y + Vector3.right.y, transform.position.z + Vector3.right.z) - transform.position);
        Vector3 targetDir = objToSnapTo.transform.forward - transform.position;
        HorizFlipUnderStander(targetDir, dir);
        VertFlipUnderStander(targetDir, dir);

        if (stumbling) {
            if (anim != null && !anim.GetBool("Stumbling")) anim.SetBool("Stumbling", true);
            Stumbler();
        } else if (invincible) {
            Invincibler();
        }

        if (!stumbling && anim != null && anim.GetBool("Stumbling")) anim.SetBool("Stumbling", false);
    }
}
