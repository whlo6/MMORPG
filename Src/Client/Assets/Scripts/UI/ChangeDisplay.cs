using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDisplay: MonoBehaviour {

	private bool ischeck;
	void Start () 
	{
		ischeck = true;
        this.gameObject.SetActive(ischeck);
	}
	
	public void Change()
	{
		ischeck = !ischeck;
        this.gameObject.SetActive(ischeck);
    }
}
