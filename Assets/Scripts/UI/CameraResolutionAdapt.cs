using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolutionAdapt : MonoBehaviour
{
    private float baseUnit = 1080;
    // Start is called before the first frame update
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = Screen.height / baseUnit * 540;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
