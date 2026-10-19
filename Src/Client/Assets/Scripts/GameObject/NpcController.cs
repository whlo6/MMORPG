using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using SkillBridge.Message;

public class NpcController : MonoBehaviour {

    public int Id;

    private Animator npcAnimation;
    private NpcDefine npc;

    private bool interactive = false;//是否处于事件执行中
    private Color orignColor;//npc骨骼原始颜色
    SkinnedMeshRenderer renderer;//npc的骨骼网格渲染器

    public Dictionary<int, QuestStatus> questList = new Dictionary<int, QuestStatus>();//该npc当前任务
    public bool hasQuest = false;//当前是否存在可接取或可提交的任务

    void Start () {
        NpcManager.Instance.npcControllers[Id - 1] = this;
        //npc骨骼处理
        this.renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        orignColor = renderer.sharedMaterial.color;
        
        this.npcAnimation = this.gameObject.GetComponent<Animator>();
        this.npc = NpcManager.Instance.GetDefine(this.Id);
        this.StartCoroutine(Actions());//启动迭代器

        //简单处理初始任务加载，现在重新加载地图会导致任务进度清空，后续用服务器解决
        if(Id==1)
        {
            questList.Add(1001, QuestStatus.InProgress);
            hasQuest = true;
        }
	}

    IEnumerator Actions()
    {
        while(true)
        {
            if (interactive)
            {
                yield return new WaitForSeconds(2f);//等2秒后再执行
            }
            else
                yield return new WaitForSeconds(Random.Range(5f, 10f));

            if(npcAnimation.GetBool("Talk")==false)//如果谈话状态已经结束，则恢复默认状态
                this.Relax();
        }
    }

    private void Relax()
    {
        npcAnimation.SetTrigger("Relax");
        npcAnimation.SetTrigger("Idle");
    }

    private void Interactive()
    {
        if(!interactive)//没有执行动作则开始执行动作
        {
            interactive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();//等待 NPC 转向玩家这个动作完成。

        //如果不是任务npc，但有任务可接取/提交，则此次对话为任务事件
        NpcDefine define = npc;
        if (define.Function != NpcFunction.InvokeQuest && this.hasQuest)
            define.Function = NpcFunction.InvokeQuest;
        //执行npc类型对应事件，并判断是否成功
        if (NpcManager.Instance.Interactive(define))
        {
            npcAnimation.SetBool("Talk",true);
        }
        yield return new WaitForSeconds(3f);
        npcAnimation.SetBool("Talk",false);
        interactive = false;//结束本次交互协程
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.gameObject.transform.position).normalized;
        while(Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward,faceTo))>5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("ClickNpcID:" + this.Id);
        Interactive();
    }

    private void OnMouseOver()
    {
        HighLight(true);
    }

    private void OnMouseEnter()
    {
        HighLight(true);
    }

    private void OnMouseExit()
    {
        HighLight(false);
    }

    //设置npc高亮
    private void HighLight(bool highLight)
    {
        if (highLight)
        {
            if (renderer.sharedMaterial.color != Color.white)
                renderer.sharedMaterial.color = Color.white;
        }
        else
        {
            if (renderer.sharedMaterial.color != orignColor)
                renderer.sharedMaterial.color = orignColor;
        }
    }
}
