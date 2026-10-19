using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAvatar : MonoBehaviour {

    public Text avatarName;
    public Text avatarLevel;
    public Slider Hp;
    public Slider Mp;
    public GameObject[] profilePictures;

    public void UpdateAvatar()
    {
        Debug.Log("UpdateAvatar");
        PlayerMessagManager.Instance.Init();
        //显示名字、等级、头像等信息
        avatarName.text = User.Instance.CurrentCharacter.Name + "\tID:" + User.Instance.CurrentCharacter.Id;
        avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
        int n = (int)User.Instance.CurrentCharacter.Class - 1;
        for(int i=0;i<3;i++)
        {
            profilePictures[i].SetActive(i == n);
        }
        //显示血条、法力
        this.Hp.maxValue = PlayerMessagManager.Instance.maxHp;
        this.Hp.value = PlayerMessagManager.Instance.hp;
        this.Mp.maxValue = PlayerMessagManager.Instance.maxMp;
        this.Mp.value = PlayerMessagManager.Instance.mp;
    }
}
