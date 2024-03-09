using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataSupplierRangedSliderDisplayer : MonoBehaviour
{
    public float min;
    public float max;
    public float speedFac = 0.01f;
    public string dataName;

    private DataSupplier dataSupplier;
    private Image sliderImage;
    private float targetAmount;
    // Start is called before the first frame update
    void Start()
    {
        dataSupplier = GameObject.FindObjectOfType<DataSupplier>();
        sliderImage = GetComponent<Image>();
        targetAmount = sliderImage.fillAmount;

        dataSupplier.Listen(dataName, OnDataDelivery);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        sliderImage.fillAmount = Mathf.Lerp(sliderImage.fillAmount, targetAmount, speedFac);
    }

    void OnDataDelivery(DataDeliveryEvent msg)
    {
        if(msg.type == 0)
        {
            targetAmount = (float.Parse(msg.data) - min) / (max - min);
        }
    }
}
