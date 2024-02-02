using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CircleSlider : MonoBehaviour
{
 
    public bool b=true;
	public Image image;
	public float speed=0.5f;
	public float factor = 100;
	public float offset = 0;


	float time =0f;
  
  public Text progress;

  
    void Update()
    {
		if(b)
		{
			time+=Time.deltaTime*speed;
			image.fillAmount= time;
			if(progress)
			{
				progress.text = ((int)(image.fillAmount*factor) + offset).ToString()/*+"%"*/;
			}
			
        if(time>1)
		{
						
			time=0;
		}
    }
	}
	
	
}
