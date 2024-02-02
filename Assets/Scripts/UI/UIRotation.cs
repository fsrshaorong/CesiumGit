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

    [Header("����������Camera")]
    public Camera cameraToLookAt;
    //[Header("ѡ����Ҫ�̶�����")]
    //[Tooltip("��������ѡ��̶�������ᣬ���õ�ѡ����None����Y")]
    //public SelectXYZ selectXYZ = SelectXYZ.None;

    void Update()
    {
        //��cameraToLookAtΪ�գ����Զ�ѡ���������
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