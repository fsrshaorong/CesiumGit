using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BlueCheckbox : MonoBehaviour, IPointerClickHandler
{
    private bool mIsOn;
    private GameObject mOnSprite;
    private GameObject mOffSprite;

    public delegate void OnCheckboxChange();

    public UnityEvent<BlueCheckbox> checkboxOn;
    public UnityEvent<BlueCheckbox> checkboxOff;
    //public event OnCheckboxChange checkBoxOn;
    //public event OnCheckboxChange checkBoxOff;

    // Start is called before the first frame update
    void Start()
    {
        mIsOn = true;
        mOnSprite = transform.Find("On").gameObject;
        mOffSprite = transform.Find("Off").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsOn()
    {
        return mIsOn;
    }

    public void SetState(bool state)
    {
        mOnSprite.SetActive(state);
        mOffSprite.SetActive(!state);
        mIsOn = state;
        ExcuteCheckboxChangeCallback(mIsOn);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        mOnSprite.SetActive(!mIsOn);
        mOffSprite.SetActive(mIsOn);
        mIsOn = !mIsOn;
        ExcuteCheckboxChangeCallback(mIsOn);
    }

    private void ExcuteCheckboxChangeCallback(bool state)
    {
        if (state)
        {
            checkboxOn.Invoke(this);
        }
        else
        {
            checkboxOff.Invoke(this);
        }
    }
}
