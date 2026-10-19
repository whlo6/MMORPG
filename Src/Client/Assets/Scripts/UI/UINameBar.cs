using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UINameBar : MonoBehaviour {

    public Text avatarName;

    public Character character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateInfo();

        this.transform.forward = Camera.main.transform.forward;//保持名字框与摄像头朝向一致
    }

    //更新信息
    void UpdateInfo()
    {
        if(character!=null)
        {
            string name = character.Name + "LV." + character.Info.Level;
            if (name != avatarName.text)//如果角色名字信息发生改变，则更新
                avatarName.text = name;
        }
    }
}
