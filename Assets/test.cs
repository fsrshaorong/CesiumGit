using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
   void Update()
   {
      // 在鼠标点击位置创建一条射线
      if (Input.GetMouseButtonDown(0))
      {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit hitInfo;

         if (Physics.Raycast(ray, out hitInfo))
         {
            // 如果射线击中了物体，执行相应的事件
            Debug.Log("触发事件：" + hitInfo.collider.gameObject.name);
            
            SceneManager.LoadScene(2, LoadSceneMode.Single);
         }
      }
   }
}
