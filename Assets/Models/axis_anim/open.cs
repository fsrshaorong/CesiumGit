using UnityEngine;

public class open : MonoBehaviour
{
    public ParticleSystem particle;
    private GameObject particleObject;
    public float timeInterval = 5.0f;  //ʱ����
    public float time = 0f; //��ʼʱ����
    private bool state = false;

    private void Start()
    {
        //particleObject = GameObject.Find("BottomSmokeInEffect");
        //particle = particleObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        int particleCount = particle.particleCount;

        if (particleCount > 0)
        {
            if (!state) // ���������������0�ҵ�ǰ״̬�ǹرգ�����д򿪲���
            {
                state = true;
                gameObject.GetComponent<Animator>().SetTrigger("open_trigger");
            }
            time += Time.deltaTime;
        }
        else
        {
            if (state && time >= timeInterval) // ���������������0�ҵ�ǰ״̬�Ǵ���ʱ�䳬����ֵ������йرղ���
            {
                state = false;
                time = 0f;
                gameObject.GetComponent<Animator>().SetTrigger("close_trigger");
            }
        }
    }

}



