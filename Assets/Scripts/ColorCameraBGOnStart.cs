using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCameraBGOnStart : MonoBehaviour
{
    [SerializeField] Color preferedColor = Color.red;
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = FindObjectOfType<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = preferedColor;
    }
}
