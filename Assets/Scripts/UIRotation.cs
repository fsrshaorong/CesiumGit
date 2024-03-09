using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour
{
    // Start is called before the first frame update
/*    public Transform UI;
    public Transform sight;
    Vector3 preDir;
    Vector3 dir;*/

/*    void Start()
    {
        preDir = new Vector3 (sight.position.x - UI.position.x, 0f, sight.position.z - UI.position.z);
        UI.rotation = Quaternion.FromToRotation(Vector3.back, preDir);
    }*/

    // Update is called once per frame
/*    void Update()
    {
        dir = new Vector3(sight.position.x - UI.position.x, 0f, sight.position.z - UI.position.z);
        UI.rotation = Quaternion.FromToRotation(preDir, dir);
    }*/

    [Header("面向的摄像机Camera")]
    public Camera cameraToLookAt;
    //[Header("选择需要固定的轴")]
    //[Tooltip("可以自由选择固定不变的轴，常用的选泽是None或者Y")]
    //public SelectXYZ selectXYZ = SelectXYZ.None;

    void Update()
    {
        //若cameraToLookAt为空，则自动选择主摄像机
        if (cameraToLookAt == null)
        {
            cameraToLookAt = Camera.main;
        }

        //Vector3 vector3 = cameraToLookAt.transform.position - transform.position;
        //switch (selectXYZ)
        //{
        //    case SelectXYZ.x:
        //        vector3.y = vector3.z = 0.0f;
        //        break;
        //    case SelectXYZ.y:
        //        vector3.x = vector3.z = 0.0f;
        //        break;
        //    case SelectXYZ.z:
        //        vector3.x = vector3.y = 0.0f;
        //        break;
        //    case SelectXYZ.None:
        //        vector3.x = vector3.y = vector3.z = 0.0f;
        //        break;
        //}
        transform.LookAt(cameraToLookAt.transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(-transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180f, 0));
        //transform.rotation = Quaternion.FromToRotation(transform.forward, vector3);
    }
}

//public enum SelectXYZ
//{
//    x,
//    y,
//    z,
//    None
//}