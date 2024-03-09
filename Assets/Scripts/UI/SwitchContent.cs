using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchContent : MonoBehaviour
{
    [NonSerialized]
    public GameObject current;
    // Start is called before the first frame update
    void Start()
    {
        current = transform.Find("EmptyPanel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchTo(GameObject target)
    {
        current.SetActive(false);
        target.SetActive(true);
        current = target;
    }
}
