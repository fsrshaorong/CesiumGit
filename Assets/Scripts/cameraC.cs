using UnityEngine;

[RequireComponent(typeof(Camera))]
public class cameraC : MonoBehaviour
{
    Camera cam;
    private IMouseBaseEvent current;
    private IMouseBaseEvent currentDrag;

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<IMouseBaseEvent>() != null)
            {
                if (current == null)
                {
                    current = hit.transform.GetComponent<IMouseBaseEvent>();
                }
                else
                {
                    if (hit.transform.GetComponent<IMouseBaseEvent>() != current)
                    {
                        current.OnMouseExit();
                        current = hit.transform.GetComponent<IMouseBaseEvent>();
                        current.OnMouseEnter();
                        //current.OnMouseHover(true);
                    }
                    else
                    {
                        current.OnMouseOver();
                    }
                }
            }
            else
            {
                if (current != null)
                {
                    current.OnMouseExit();
                    current = null;
                }
            }
        }
    }
}