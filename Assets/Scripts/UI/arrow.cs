using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class arrow : MonoBehaviour
{

    public Transform MainCamera;
    //public Vector3 offset;
    
    // Update is called once per frame
    void Update()
    {
        //transform.position = MainCamera.position + offset;
        //Vector3 rot = new Vector3(90, MainCamera.eulerAngles.y, 0);
        var position = MainCamera.position;
        transform.position = new Vector3(position.x, 80, position.z);

        //transform.rotation = Quaternion.Euler(rot);
    }
}
