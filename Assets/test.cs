using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
   public CameraMovements cameraMovements;

   private bool isDragging;

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

            if (hitInfo.collider.gameObject.name == "Icon_location01_White")
            {
               SceneManager.LoadScene(2, LoadSceneMode.Single);
            }

            if (hitInfo.collider.gameObject.name == "厂房5")
            {
               isDragging = true;
            }
         }

      }
   }
}
