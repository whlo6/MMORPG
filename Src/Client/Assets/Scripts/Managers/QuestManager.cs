using Assets.Scripts.Managers;
using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public Dictionary<int, QuestStatus> quests = new Dictionary<int, QuestStatus>();
    public UIQuest uiQuest;
    public UIDialogBox uiDialogBox;

    public void Init()
    {
        NpcManager.Instance.RegisterNpcEvent(NpcFunction.InvokeQuest, AddQuest);
    }

    bool AddQuest(NpcDefine npc)
    {
        NpcController[] npcControllers = NpcManager.Instance.npcControllers;
        foreach (var quest in npcControllers[npc.ID-1].questList.ToList())
        {
            QuestDefine define = DataManager.Instance.Quests[quest.Key];
            //如果当前对话的npc的任务处于接取未完成状态
            if (quest.Value== QuestStatus.InProgress)
            {
                if (this.quests.ContainsKey(quest.Key))
                {
                    UIManager.Instance.Show<UIDialogBox>().Init(define.DialogAccept);//播放已接受任务对话
                }
                else//玩家任务列表中不存在该任务（等效于首次接取该任务）
                {
                    //为对应提交npc添加任务
                    QuestStatus state;
                    //如果该任务不存在任务目标，则直接为已完成状态
                    if (define.Target1 == QuestTarget.None)
                    {
                        state = QuestStatus.Complated;
                    }
                    else
                        state = QuestStatus.InProgress;
                    //为提交npc添加任务
                    if (!npcControllers[define.SubmitNPC - 1].questList.ContainsKey(quest.Key))
                        npcControllers[define.SubmitNPC - 1].questList.Add(quest.Key, state);
                    else
                        npcControllers[define.SubmitNPC - 1].questList[quest.Key] = state;

                    npcControllers[define.SubmitNPC - 1].hasQuest = true;
                    npcControllers[npc.ID - 1].hasQuest = false;

                    //将该任务添加至玩家任务列表
                    quests.Add(quest.Key, state);
                    if (QuestManager.Instance.uiQuest != null)
                        QuestManager.Instance.uiQuest.Init();
                    UIManager.Instance.Show<UIDialogBox>().Init(define.Dialog);//播放任务对话
                }
            }
                //如果当前对话的npc的任务处于待提交状态且玩家任务列表中存在该任务
            else if(quest.Value==QuestStatus.Complated&&this.quests.ContainsKey(quest.Key))
            {
                //将任务相关npc设置为完成状态
                npcControllers[define.AcceptNPC - 1].questList[quest.Key] = QuestStatus.Finished;
                npcControllers[npc.ID - 1].questList[quest.Key] = QuestStatus.Finished;
                this.quests[quest.Key] = QuestStatus.Finished;

                UIManager.Instance.Show<UIDialogBox>().Init(define.DialogFinish);//播放任务提交对话
                npcControllers[npc.ID - 1].hasQuest = false;

                //添加奖励
                ItemManager.Instance.gold += define.RewardGold;

                //为相关npc添加后续任务
                if (define.PostQuest!=0)
                {
                    define = DataManager.Instance.Quests[define.PostQuest];
                    //为任务接取npc添加任务
                    npcControllers[define.AcceptNPC - 1].questList.Add(define.ID, QuestStatus.InProgress);
                    npcControllers[define.AcceptNPC - 1].hasQuest = true;
                }
            }
        }
        return true;
    }
}
