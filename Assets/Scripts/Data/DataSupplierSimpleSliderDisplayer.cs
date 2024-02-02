using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataSupplierSimpleSliderDisplayer : MonoBehaviour
{
    public string dataName;

    private DataSupplier dataSupplier;
    private Image sliderImage;
    private float targetFillAmount;
    // Start is called before the first frame update
    void Start()
    {
        dataSupplier = GameObject.FindObjectOfType<DataSupplier>();
        sliderImage = GetComponent<Image>();
        dataSupplier.Listen(dataName, OnDataDelivery);
        targetFillAmount = sliderImage.fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        //sliderImage.fillAmount = Mathf.Lerp(sliderImage.fillAmount, targetFillAmount, 0.01f);
    }

    private void FixedUpdate()
    {
        sliderImage.fillAmount = Mathf.Lerp(sliderImage.fillAmount, targetFillAmount, 0.01f);
    }

    void OnDataDelivery(DataDeliveryEvent msg)
    {
        if(msg.type == 0)
        {
            targetFillAmount = float.Parse(msg.data) % 1.0f;
        }
    }
}
