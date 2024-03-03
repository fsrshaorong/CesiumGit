using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBuildingAndFactory : MonoBehaviour
{
    public void  LoadBuilding_Click()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
    public void  LoadFactory_Click()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}
