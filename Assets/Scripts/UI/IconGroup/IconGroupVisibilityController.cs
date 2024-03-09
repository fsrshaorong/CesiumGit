using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconGroupVisibilityController : MonoBehaviour
{
    CanvasGroup canvasGroup;

    public float switchDuration;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hide()
    {
        canvasGroup.DOFade(0, switchDuration).onComplete =
            () =>
            {
                //gameObject.SetActive(false);
                for(int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    child.GetComponentInChildren<BoxCollider>().enabled = false;
                }
            };
    }

    public void show()
    {
        //gameObject.SetActive(true);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.GetComponentInChildren<BoxCollider>().enabled = true;
        }
        canvasGroup.DOFade(1, switchDuration);
    }
}
