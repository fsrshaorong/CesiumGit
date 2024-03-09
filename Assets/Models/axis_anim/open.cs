using UnityEngine;

public class open : MonoBehaviour
{
    public ParticleSystem particle;
    private GameObject particleObject;
    public float timeInterval = 5.0f;  //时间间隔
    public float time = 0f; //起始时间数
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
            if (!state) // 如果粒子数量大于0且当前状态是关闭，则进行打开操作
            {
                state = true;
                gameObject.GetComponent<Animator>().SetTrigger("open_trigger");
            }
            time += Time.deltaTime;
        }
        else
        {
            if (state && time >= timeInterval) // 如果粒子数量等于0且当前状态是打开且时间超过阈值，则进行关闭操作
            {
                state = false;
                time = 0f;
                gameObject.GetComponent<Animator>().SetTrigger("close_trigger");
            }
        }
    }

}



