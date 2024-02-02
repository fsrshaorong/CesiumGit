using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ScreenSwitch : MonoBehaviour
{
    public TMP_Text buttonText;
    public Transform bigScreen;
    public Transform camMainScrn;
    //public FlameSwitch flameScript;

    public float switchDuration = 0.5f;
    public bool withCharts = true;

    public GameObject open;
    public GameObject close;

    private Dictionary<string, float> trsImage = new Dictionary<string, float>();
    private Dictionary<string, float> trsText = new Dictionary<string, float>();
    private Text[] texts;
    private Image[] images;
    private Transform panel;
    private bool toCharts = false;
    private bool toNoCharts = false;
    private Vector3 posBig = new(-100f, 150f, -100f);
    private Vector3 rota = new(50f, 0f, 0f);
    private Vector3 posNorm = new(-100f, 110f, -40f);
    private List<Vector3> initPositions = new List<Vector3>();
    private GameObject panelFolder;


    public void OnButtonClick()
    {
        if (withCharts) //����Ǵ���ģʽ�����л�Ϊֻ��ģ��ģʽ
        {
            withCharts = !withCharts;
            toNoCharts = true;
            buttonText.text = "Ҥ¯ģʽ";
            open.SetActive(true);
            close.SetActive(false);
        }
        else
        {
            withCharts = !withCharts;
            toCharts = true;
            buttonText.text = "��������ģʽ";
            open.SetActive(false);
            close.SetActive(true);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        panelFolder = bigScreen.transform.Find("EventPanelFolder").gameObject;

        for (int i = 0; i < panelFolder.transform.childCount; i++)
        {
            initPositions.Add(panelFolder.transform.GetChild(i).localPosition);
        }

        for (int i = 0; i < panelFolder.transform.childCount; i++)
        {
            panel = panelFolder.transform.GetChild(i);
            images = panel.GetComponentsInChildren<Image>();
            texts = panel.GetComponentsInChildren<Text>();
            int j = 0;
            foreach (Image image in images)
            {
                string key = i.ToString() + j.ToString();
                trsImage.Add(key, image.color.a);
                j++;
            }
            int q = 0;
            foreach (Text text in texts)
            {
                string key = i.ToString() + q.ToString();
                trsText.Add(key, text.color.a);
                q++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (toNoCharts)
        {
            //���λ����ת�ı�
            camMainScrn.DOLocalRotate(rota, switchDuration);
            camMainScrn.DOMove(posNorm, switchDuration);

            //�ı�ͼ��λ�ü�͸����
            for (int i = 0; i < panelFolder.transform.childCount; i++)
            {
                panel = panelFolder.transform.GetChild(i);
                panel.DOLocalMove(initPositions[i] * 1.5f, switchDuration);
                panel.DOScale(new Vector3(1.5f, 1.5f, 1.5f), switchDuration);
                images = panel.GetComponentsInChildren<Image>();
                texts = panel.GetComponentsInChildren<Text>();
                foreach (Image image in images)
                {
                    image.DOFade(0f, switchDuration);
                }
                foreach (Text text in texts)
                {
                    text.DOFade(0f, switchDuration);
                }
            }

            //�������Ŀ��λ����ֹͣ�任
            if ((camMainScrn.position - posNorm).magnitude < 100f)
            {
                toNoCharts = false;
            }
        }

        if (toCharts)
        {
            //���λ����ת�ı�
            camMainScrn.DOLocalRotate(rota, switchDuration);
            camMainScrn.DOMove(posBig, switchDuration);

            //�ı�ͼ��λ�ü�͸����
            for (int i = 0; i < panelFolder.transform.childCount; i++)
            {
                panel = panelFolder.transform.GetChild(i);
                panel.DOLocalMove(initPositions[i], switchDuration);
                panel.DOScale(new Vector3(1, 1, 1), switchDuration);
                images = panel.GetComponentsInChildren<Image>();
                texts = panel.GetComponentsInChildren<Text>();
                int j = 0;
                foreach (Image image in images)
                {
                    string key = i.ToString() + j.ToString();
                    image.DOFade(trsImage[key], switchDuration);
                    j++;
                }
                int q = 0;
                foreach (Text text in texts)
                {
                    string key = i.ToString() + q.ToString();
                    text.DOFade(trsText[key], switchDuration);
                    q++;
                }
            }

            //�������Ŀ��λ����ֹͣ�任
            if ((camMainScrn.position - posBig).magnitude < 100f)
            {
                toCharts = false;
            }
        }
    }
}
