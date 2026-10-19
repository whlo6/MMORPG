using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {

    //用于退出游戏按钮，作用为隐藏当前应用程序
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public UIAvatar uIAvatar;
    public GameObject uiLoading;

    // Use this for initialization
    protected override void OnStart () {
        uIAvatar.UpdateAvatar();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BackToCharSelect()
    {
        Debug.Log("BackToCharSelect");

        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
        this.global = false;
        Destroy(this.gameObject);
    }

    //打开背包
    public void OnClickBag()
    {
        BagManager.Instance.uiBag = UIManager.Instance.Show<UIBag>();
        if (BagManager.Instance.uiBag.isChange)
            BagManager.Instance.uiBag.ChangeBag();
    }

    public void OnClickQuest()
    {
        QuestManager.Instance.uiQuest = UIManager.Instance.Show<UIQuest>();
    }

    public void OnClickQuitGame()
    {
        Services.UserService.Instance.SendGameLeave();
        // 1. 隐藏窗口
        #if UNITY_STANDALONE_WIN
                IntPtr hwnd = GetActiveWindow();
                ShowWindow(hwnd, 0);
        #endif
        // 2. 开启协程，等待2秒后退出程序
        StartCoroutine(DelayQuitGame(2f));
    }

    IEnumerator DelayQuitGame(float waitTime)
    {
        Debug.Log("退出游戏");
        yield return new WaitForSecondsRealtime(waitTime);
        // 2秒后关闭应用
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
