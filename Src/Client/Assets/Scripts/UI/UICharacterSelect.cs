using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;
using SkillBridge.Message;

public class UICharacterSelect : MonoBehaviour {

    public InputField charName;//创建角色界面--玩家名字
    CharacterClass charClass;//创建角色界面--玩家职业类型

    public GameObject uiCharacterCreate;
    public GameObject uiCharacterSelect;

    public Transform uiCharList;
    public GameObject uiCharInfo;

    public List<GameObject> uiChars = new List<GameObject>();

    public Text descs;//描述

    string[] names = new string[3];//数据库读取到的职业描述名字

    int selectCharacterIdx = 0;//职业选中ID

    public UICharacterView characterView;//角色模型显示面板
   
    // Use this for initialization
    void Start()
    {
        InitCharacterSelect(true);
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;

        DataManager.Instance.Load();

        //从data表中读取职业昵称
        for (int i = 0; i < 3; i++)
        {
            names[i] = DataManager.Instance.Characters[i + 1].Name;
        }
        descs.text = names[0] + "\n" + DataManager.Instance.Characters[1].Description;//更改描述
    }

    public void InitCharacterCreate()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickCreate()//创建角色界面--Button创建角色
    {
        if(string.IsNullOrEmpty(this.charName.text))
        {
            MessageBox.Show("请输入角色昵称");
        }
        else
        {
            if (this.charClass == CharacterClass.None) this.charClass = CharacterClass.Warrior;
            UserService.Instance.SendCharacterCreate(this.charName.text, this.charClass);
        }
    }

    void OnCharacterCreate(Result result, string message)
    {
        if (result == Result.Success)
        {
            MessageBox.Show("成功");
            //MessageBox.Show(User.Instance.Info.Player.Characters.Count.ToString());
            InitCharacterSelect(true);

        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }

    public void InitCharacterSelect(bool init)//初始化角色选择界面
    {
        Debug.Log("InitCharacterSelect");

        uiCharacterCreate.SetActive(false);
        uiCharacterSelect.SetActive(true);

        if (init)
        {
            foreach (var old in uiChars)
            {
                Destroy(old);
            }
            uiChars.Clear();

            for(int i=0;i< User.Instance.Info.Player.Characters.Count; i++)
            {
                GameObject go = Instantiate(uiCharInfo, uiCharList);
                UICharInfo charInfo = go.GetComponent<UICharInfo>();
                charInfo.info = User.Instance.Info.Player.Characters[i];
                
                Button btn = go.GetComponent<Button>();
                int idx = i;
                btn.onClick.AddListener(() =>
                {
                    OnSelectCharacter(idx);
                });

                uiChars.Add(go);
                go.SetActive(true);
            }

            //根据已创建角色的数量自动调整选择栏的长度
            RectTransform rectTransform = uiCharList.gameObject.GetComponent<RectTransform>();
            Rect rect = rectTransform.rect;
            rect.height = 15 + 78 * User.Instance.Info.Player.Characters.Count;
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rect.height);

            //自改--自改--自改--自改--自改--自改--自改
            selectCharacterIdx = User.Instance.selectCharacterIdx;//更新选中角色
            //自改--自改--自改--自改--自改--自改--自改

            //默认选中第一个角色,如果没有创建角色则跳转创建界面
            if (User.Instance.Info.Player.Characters.Count != 0)
                OnSelectCharacter(selectCharacterIdx);
            else
            {
                uiCharacterCreate.SetActive(true);
                uiCharacterSelect.SetActive(false);
            }
        }
    }

    //进入创建角色界面
    public void OnSelectClass(int charClass)//创建角色时选择职业
    {
        Debug.Log("try create" + names[charClass]);

        this.charClass = (CharacterClass)(charClass+1);

        characterView.CurrectCharacter = charClass;//更新选中状态并更新显示模型

        descs.text = names[charClass] + "\n" + DataManager.Instance.Characters[charClass+1].Description;//更改描述
        //注册表中的下标从1开始
    }

    public void OnSelectCharacter(int idx)//切换角色
    {
        User.Instance.selectCharacterIdx = idx;//更新User中的选中角色
        this.selectCharacterIdx = idx;//更新选中角色
        var cha = User.Instance.Info.Player.Characters[idx];
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        characterView.CurrectCharacter = (int)cha.Class -1;
        
        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            UICharInfo charInfo = uiChars[i].GetComponent<UICharInfo>();
            charInfo.Selected = idx == i;
        }
    }
    public void OnClickPlay()
    {
        if (selectCharacterIdx >= 0)
        {
            UserService.Instance.SendGameEnter(selectCharacterIdx);
            //MessageBox.Show("进入游戏", "进入游戏", MessageBoxType.Confirm);
        }
    }
}
