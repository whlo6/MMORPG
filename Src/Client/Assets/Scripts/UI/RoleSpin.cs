using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class RoleSpin : MonoBehaviour 
{
	public GameObject role;

	private float dics;
	private Scrollbar spin;

	void Start () 
	{
		spin = GetComponent<Scrollbar>();
	}
	
	void Update () 
	{
        if(spin.value!=dics)
		{
			float spin_y= spin.value * -5;//计算旋转后的角度

            role.transform.Rotate(new Vector3(role.transform.rotation.x, spin_y, role.transform.rotation.z));
			spin.value = dics;
		}
    }
}
