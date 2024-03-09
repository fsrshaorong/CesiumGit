 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OilGauge : MonoBehaviour
{

    public bool b = true;
    public Image image;
    public float speed = 0.5f;

    float time = 0f;

    public Text progress;

    public Transform oilOilGaugePivot;

    public string dataName;

    private DataSupplier dataSupplier;
    private float targetAmount;

    void Start()
    {
        dataSupplier = GameObject.FindObjectOfType<DataSupplier>();
        dataSupplier.Listen(dataName, OnDataDelivery);
        targetAmount = image.fillAmount;
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (b)
        {
            //time += Time.deltaTime * speed;

            //image.fillAmount = time * 0.8f + 0.1f;

            image.fillAmount = Mathf.Lerp(image.fillAmount, targetAmount, 0.01f);


            //oilOilGaugePivot.localEulerAngles = Vector3.forward * (128f - 256 * time);
            oilOilGaugePivot.localEulerAngles = Vector3.forward * (128f - 256f * (image.fillAmount - 0.1f) * 1.25f);

            if (progress)
            {
                //progress.text = (int)(image.fillAmount * 100) + "%";
                progress.text = ((int)targetAmount).ToString() + "%";
            }

            if (time > 1)
            {
                time = 0;
            }
        }
    }

    void OnDataDelivery(DataDeliveryEvent msg)
    {
        if(msg.type == 0)
        {
            targetAmount = 0.1f + float.Parse(msg.data) * 0.008f;
            //targetAmount = 1.0f - targetAmount;
        }
    }
}
