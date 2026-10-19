using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuest : UIWindow {

    public Transform[] questList;//任务列表的父节点
    public GameObject questItem;
    public Dictionary<int, UIQuestItem> myQuestItems = new Dictionary<int, UIQuestItem>();

    public Text TQuestName;
    public Text TOverview;
    public Text TState;
    public Text TReward;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        foreach (var quest in QuestManager.Instance.quests)
        {
            GameObject obj = Instantiate(this.questItem);
            if(myQuestItems.ContainsKey(quest.Key))
            {
                Destroy(myQuestItems[quest.Key].gameObject);
                myQuestItems.Remove(quest.Key);
            }
            myQuestItems.Add(quest.Key, obj.GetComponent<UIQuestItem>());
            myQuestItems[quest.Key].Init(quest.Key);//初始化任务信息
            //根据任务类型挂载到对应任务节点
            if (myQuestItems[quest.Key].define.Type == QuestType.Main)
                obj.transform.SetParent(questList[0]);
            else
                obj.transform.SetParent(questList[1]);
        }
    }

    public void SetMessageBar(string questName,string overview,string state,string reward)
    {
        this.TQuestName.text = questName;
        this.TOverview.text = overview;
        this.TState.text = state;
        this.TReward.text = reward;
    }
}
