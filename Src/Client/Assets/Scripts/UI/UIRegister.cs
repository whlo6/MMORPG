using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UIRegister : MonoBehaviour 
{
    public InputField username;
    public InputField password;
    public InputField passwordConfirm;
    public Button buttonRegister;

    public GameObject uiLogin;

    // Use this for initialization
    void Start () {
        UserService.Instance.OnRegister = OnRegister;//监听服务器注册请求结果
    }

    // Update is called once per frame
    void Update () {
		
	}

	public void OnClickRegister()
	{
        //string.IsNullOrEmpty(mail.text)可判断文本UI中是否存在字符
        if (string.IsNullOrEmpty(username.text))
		{
			MessageBox.Show("邮箱不能为空！");
			return;
		}
		else if(string.IsNullOrEmpty(password.text))
		{
            MessageBox.Show("密码不能为空！");
            return;
        }
        else if (string.IsNullOrEmpty(passwordConfirm.text))
        {
            MessageBox.Show("请再输入一次确认密码！");
            return;
        }
		else if(password.text!= passwordConfirm.text)
		{
            MessageBox.Show("两次输入密码不一致！");
            return;
        }

        //将注册请求发送至服务器，处理结果由Start()内的UserService.Instance.OnRegisterUserService.Instance.OnRegister监听
        UserService.Instance.SendRegister(this.username.text, this.password.text);
    }

    void OnRegister(Result result, string message)
    {
        if (result == Result.Success)
        {
            //登录成功，进入角色选择
            MessageBox.Show("注册成功,请登录", "提示", MessageBoxType.Information).OnYes = this.CloseRegister;
            CloseRegister();
        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }

    void CloseRegister()
    {
        this.gameObject.SetActive(false);
        uiLogin.SetActive(true);
    }

}
