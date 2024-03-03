using System.Collections;
using System.Collections.Generic;
using CesiumForUnity;
using Unity.Mathematics;
using UnityEngine;

public class DynamicCameraRotate : MonoBehaviour
{
    private flyToController flyToController;
    
    private double3 position;

    public CesiumFlyToController Controller;

    private int lon=116;
    private int lat=39;
    private double height = 5000000;
    void Start()
    {
        flyToController=GetComponent<flyToController>();
        position = new double3(lon,lat,height);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (flyToController.canRotate)
        {
            CameraRotate();
            lon += 1;
            position = new double3(lon,lat,height);
        }
    }

    void CameraRotate()
    {
        Controller.FlyToLocationLongitudeLatitudeHeight(position,0,90,true);
    }
}
