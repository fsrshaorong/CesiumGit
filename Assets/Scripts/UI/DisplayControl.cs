using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            Screen.SetResolution(Display.displays[i].renderingWidth, Display.displays[i].renderingHeight, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
