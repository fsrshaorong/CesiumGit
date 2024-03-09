using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFurnaceCamera : MonoBehaviour
{
    private Camera cam;
    private float oriPosX;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        oriPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = new Vector3(oriPosX + 50 * Mathf.Sin(Time.time), cam.transform.position.y, cam.transform.position.z);
        //cam.Render();
    }
}
