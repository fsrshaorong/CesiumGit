using System.Collections;
using System.Collections.Generic;
using CesiumForUnity;
using Unity.Mathematics;
using UnityEngine;

public class flyToController : MonoBehaviour
{
    public List<CesiumSubScene> SubScenes = new List<CesiumSubScene>();

    public List<Vector2> subSceneYawAndPitch = new List<Vector2>();

    public CesiumFlyToController Controller;

    private int fly = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fly > -1)
        {
            flyToScenes(fly);
        }
        fly = -1;
    }

    void flyToScenes(int index)
    {
        CesiumSubScene subScene = SubScenes[index];
        double3 coordinatesECEF = new double3(subScene.ecefX,subScene.ecefY,subScene.ecefZ);
        Vector2 yawAndPitch = Vector2.zero;
        if (index<subSceneYawAndPitch.Count)
        {
            yawAndPitch = subSceneYawAndPitch[index];
        }

        if (Controller != null)
        {
            Controller.FlyToLocationEarthCenteredEarthFixed(coordinatesECEF, yawAndPitch.x, yawAndPitch.y,
                true);
        }
    }
    
    public void FlyTo_Click()
    {
        fly = 0;
    }

    public void FlySceondScene_Click()
    {
        fly = 1;
    }

    public void FlyCameraScene_Click()
    {
        fly = 2;
    }
}
