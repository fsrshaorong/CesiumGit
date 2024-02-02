using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlameSwitch : MonoBehaviour
{
    public float flameDuration = 6f;
    public float flameInterval = 6f;
    public Light[] lights;

    public Transform[] smoke;

    List<ParticleSystem> smokes = new List<ParticleSystem>();

    float intense;
    bool isOn;
    bool isFirstFour;

    float lastOnTime;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem[] particleSystems = transform.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particleSystems)
        {
            particle.Stop();
        }
        lastOnTime = Time.time;
        TurnOn();
        //InvokeRepeating("TurnOn", 0f, flameDuration + flameInterval);
        //InvokeRepeating("TurnOff", flameDuration, flameDuration + flameInterval);

        foreach (var item in smoke)
        {
            foreach (var particle in item.GetComponentsInChildren<ParticleSystem>())
            {
                smokes.Add(particle);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            if (Time.time - lastOnTime > flameDuration)
            {
                lastOnTime = Time.time;
                TurnOff();
            }
        }
        if (!isOn)
        {
            if (Time.time - lastOnTime > flameInterval)
            {
                lastOnTime = Time.time;
                TurnOn();
            }
        }
    }

    void TurnOn()
    {
        isOn = true;
        LightsDoLerp();
        isFirstFour = !isFirstFour;
        if (isFirstFour) //ÄÏÇ½Ð¡Â¯Åç»ð
        {
            //¿ØÖÆ»ðÑæ²¥·Å
            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(i).GetComponent<ParticleSystem>().Play();
            }
            //ÄÏÇ½in±±Ç½out
            foreach (var particle in smokes)
            {
                string tag = particle.tag;
                switch (tag)
                {
                    case "SouthOut":
                        particle.Stop();
                        break;
                    case "SouthIn":
                        particle.Play();
                        break;
                    case "NorthOut":
                        particle.Play();
                        break;
                    case "NorthIn":
                        particle.Stop();
                        break;
                }
            }
        }
        if (!isFirstFour) //±±Ç½Ð¡Â¯Åç»ð
        {
            //¿ØÖÆ»ðÑæ²¥·Å
            for (int i = 4; i < 8; i++)
            {
                transform.GetChild(i).GetComponent<ParticleSystem>().Play();
            }
            //ÄÏÇ½out±±Ç½in
            foreach (var particle in smokes)
            {
                string tag = particle.tag;
                switch (tag)
                {
                    case "SouthOut":
                        particle.Play();
                        break;
                    case "SouthIn":
                        particle.Stop();
                        break;
                    case "NorthOut":
                        particle.Stop();
                        break;
                    case "NorthIn":
                        particle.Play();
                        break;
                }
            }
        }
    }

    void TurnOff()
    {
        isOn = false;
        LightsDoLerp();
        for (int i = 0; i < 8; i++)
        {
            transform.GetChild(i).GetComponent<ParticleSystem>().Stop();
        }

        foreach (var particle in smokes)
        {
            particle.Stop();
        }
    }

    void LightsDoLerp()
    {
        if (isOn)
        {
            intense = 1000f;
        }
        else
        {
            intense = 0f;
        }
        foreach (Light light in lights)
        {
            DOTween.To(() => light.intensity, x => { light.intensity = x; }, intense, flameDuration * 0.2f);
        }
    }
}
