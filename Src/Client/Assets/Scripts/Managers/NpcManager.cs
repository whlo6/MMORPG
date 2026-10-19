using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : Singleton<NpcManager> {

    public delegate bool NpcActionHandler(NpcDefine npc);//自定义一个npc动作事件

    public Dictionary<NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();

    public NpcController[] npcControllers = new NpcController[4];

    //注册NPC事件
    public void RegisterNpcEvent(NpcFunction function,NpcActionHandler action)
    {
        if(!this.eventMap.ContainsKey(function))
        {
            this.eventMap[function] = action;
        }
        else
        {
            this.eventMap[function] += action;
        }
    }

    public NpcDefine GetDefine(int id)
    {
        return DataManager.Instance.Npcs[id];
    }

    public bool Interactive(int npcId)
    {
        if(DataManager.Instance.Npcs.ContainsKey(npcId))
        {
            var npc = DataManager.Instance.Npcs[npcId];
            return Interactive(npc);
        }
        return false;
    }

    //执行对应类型npc的事件
    public bool Interactive(NpcDefine npc)
    {
        if(npc.Type==NpcType.Task)//任务npc
        {
            return DoTaskInterActive(npc);
        }
        if(npc.Type==NpcType.Functional)//功能npc
        {
            return DoFunctionInterActive(npc);
        }
        return false;
    }

    //任务npc事件
    public bool DoTaskInterActive(NpcDefine npc)
    {
        //MessageBox.Show("点击了Npc" + npc.Name + "进行对话");
        return this.eventMap[npc.Function](npc);
    }

    //功能npc事件
    public bool DoFunctionInterActive(NpcDefine npc)
    {
        if (npc.Type != NpcType.Functional) return false;
        if (!this.eventMap.ContainsKey(npc.Function)) return false;

        return this.eventMap[npc.Function](npc);
    }
}
