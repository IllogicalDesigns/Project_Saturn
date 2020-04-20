using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumbleDianaEffects : MonoBehaviour
{
    bool stumbling = false;
    [SerializeField] SpriteRenderer sr;
    float timeToStumble = 0.1f;
    float stumbleTimer;
    [SerializeField] Color stumbleColor;

    public void EnteredStumble() {
        stumbling = true;
    }

    public void ExitedStumble() {
        stumbling = false;
    }

    private void Update() {
        if (stumbling)
            stumbleTimer -= Time.deltaTime;

        if(stumbling && stumbleTimer <= 0) {
            stumbleTimer = timeToStumble;
            if (sr.color == stumbleColor)
                sr.color = Color.white;
            else
                sr.color = stumbleColor;
        }
    }
}
