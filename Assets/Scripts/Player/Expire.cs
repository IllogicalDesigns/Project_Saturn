using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expire : MonoBehaviour {

    [SerializeField] public float ExistTime;
    private float createTime;
    
    // Start is called before the first frame update
    void Start() {
        createTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time < createTime + ExistTime) return;
        
        Destroy(gameObject);
    }
}