using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashOnDamage : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    Color orig;
    [SerializeField] Color stumbColor;
    bool stumbling;

    float stuTimer, timeInbetweenStums = 0.1f;

    private void Start() {
        orig = spriteRenderer.color;
    }

    IEnumerator flashOnce() {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = orig;
    }

    public void OnDamage() {
        StartCoroutine(flashOnce());
    }

    private void Update() {
        if(stumbling && stuTimer <= 0) {
            if (spriteRenderer.color == orig)
                spriteRenderer.color = stumbColor;
            else
                spriteRenderer.color = orig;

            stuTimer = timeInbetweenStums;
        }

        if (stumbling && stuTimer > 0)
            stuTimer -= Time.deltaTime;
    }

    public void EnteredStumble() {
        stumbling = true;
    }

    public void ExitedStumble() {
        stumbling = false;
        spriteRenderer.color = orig;
    }
}
