using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoding : MonoBehaviour 
{
	public Slider slider;
    public float lodingSpeed;

    bool isLoading = false;

    public void LoadingStart()
    {
        slider.value = 1f;
        isLoading = true;
    }

    private void Update () 
	{
        if (!isLoading) this.LoadingStart();
        if (slider.value<100f)
        {
            slider.value += lodingSpeed;
        }
        else
        {
            isLoading = false;
            this.gameObject.SetActive(false);
        }
    }
	
}
