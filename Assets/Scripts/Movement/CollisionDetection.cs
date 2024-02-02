using UnityEngine;
public class CollisionDetection : MonoBehaviour
{
    public Camera cameraToCheck;

    private void Update()
    {
        if (cameraToCheck == null)
        {
            Debug.LogWarning("Please assign a Camera to check in the inspector!");
            return;
        }

        RaycastHit hit;
        bool isHit = Physics.Raycast(cameraToCheck.transform.position, cameraToCheck.transform.forward, out hit);

        if (isHit)
        {
            cameraToCheck.transform.position = hit.point - cameraToCheck.transform.forward * 0.1f;
        }
        
        Debug.DrawRay(cameraToCheck.transform.position, cameraToCheck.transform.forward * 10f, isHit ? Color.red : Color.green);
    }
}



