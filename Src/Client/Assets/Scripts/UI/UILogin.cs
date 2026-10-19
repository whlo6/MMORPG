using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UILogin : MonoBehaviour 
{
    public InputField username;
    public InputField password;

    private void Start () 
	{
        UserService.Instance.OnLogin = OnLogin;//挂载一个监听事件
    }
    private void Update () 
	{
		
	}

    public void OnClickLogin()
    {
        //string.IsNullOrEmpty(mail.text)可判断文本UI中是否存在字符
        if (string.IsNullOrEmpty(username.text))
        {
            MessageBox.Show("邮箱不能为空！");
            return;
        }
        else if (string.IsNullOrEmpty(password.text))
        {
            MessageBox.Show("密码不能为空！");
            return;
        }

        UserService.Instance.SendLogin(this.username.text, this.password.text);
    }

    void OnLogin(Result result, string message)
    {
        if (result == Result.Success)
        {
            //登录成功，进入角色选择
            Debug.LogFormat("LoginSucces:{0},{1}", username.text, password.text);
            SceneManager.Instance.LoadScene("CharSelect");
        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }

    void CloseLogin()
    {
        //this.gameObject.SetActive(false);
        //uiLogin.SetActive(true);
    }

}
