using Assets.Scripts.Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : Singleton<BagManager> {

    public int Unlocked;//单背包页格子数
    public BagItem[] Items;
    public UIBag uiBag;
    public Dictionary<int, GameObject> bagObj = new Dictionary<int, GameObject>();
    public UIBagItem checkObj;

    NBagInfo info;
    
    unsafe public void Init(NBagInfo info)
    {
        Debug.Log("InitBagManager");

        this.info = info;
        this.Unlocked = info.Unlocked;
        Items = new BagItem[this.Unlocked];
        if(info.Items!=null&&info.Items.Length>=this.Unlocked)
        {
            Analyze(info.Items);//指针错位处理
        }
        else
        {
            info.Items = new byte[sizeof(BagItem) * this.Unlocked];
            Reset();//读取背包物体
        }
    }

    public void Reset()
    {
        int i = 0;
        foreach(var kv in ItemManager.Instance.Items)
        {
            int count = kv.Value.Count;
            //判断该物品单组是否超过上限
            if (kv.Value.Count>kv.Value.Define.StackLimit)
            {
                //将超过单组上限的物体拆分为多组
                while(count>kv.Value.Define.StackLimit && i< this.Unlocked)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Define.StackLimit;
                    count -= kv.Value.Define.StackLimit;

                    i++;
                }
            }

            if (i >= this.Unlocked) break;//超出背包上限的物体不处理

            this.Items[i].ItemId = (ushort)kv.Key;
            this.Items[i].Count = (ushort)count;
            i++;
        }
    }

    unsafe void Analyze(byte[] data)
    {
        fixed(byte* pt=data)
        {
            for(int i=0;i<this.Unlocked;i++)
            {
                BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                Items[i] = *item;
            }
        }
    }

    unsafe public NBagInfo GetBagInfo()
    {
        fixed (byte* pt = info.Items)
        {
            for (int i = 0; i < this.Unlocked; i++)
            {
                BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                *item = Items[i];
            }
        }
        return this.info;
    }
}
