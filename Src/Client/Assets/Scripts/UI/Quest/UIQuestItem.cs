using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.UIElements;
using SkillBridge.Message;
using Common.Data;

public class UIQuestItem :MonoBehaviour
{
    public int Id;
    public QuestStatus state;
    public QuestDefine define;

    public Text TName;
	
    public void Init(int id)
    {
        this.Id = id;
        state = QuestManager.Instance.quests[id];
        define = DataManager.Instance.Quests[id];

        this.TName.text = define.Name;
    }

    public void OnClick()
    {
        string strState = "";
        switch(state)
        {
            case QuestStatus.InProgress:
                strState = "进行中";
                break;
            case QuestStatus.Complated:
                strState = "可提交";
                break;
            case QuestStatus.Finished:
                strState = "已完成";
                break;
            case QuestStatus.Faled:
                strState = "已失败";
                break;
        }

        string strReward = "";
        if (define.RewardExp > 0) strReward = strReward + "经验 x " + define.RewardExp.ToString() + "\n";
        if (define.RewardGold > 0) strReward = strReward + "金币 x " + define.RewardExp.ToString() + "\n";
        //道具奖励暂不显示

        QuestManager.Instance.uiQuest.SetMessageBar(define.Name, define.Overview, strState, strReward);
    }
}
