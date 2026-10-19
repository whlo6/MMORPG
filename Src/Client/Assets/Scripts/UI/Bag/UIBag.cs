using Assets.Scripts.Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : UIWindow {

    public Text gold;//金币
    public GameObject describeBar;
    public Text TName;
    public Text TType;
    public Text TDescribe;
    public Transform[] pages;//格子的位置
    public Transform[] bagList;//背包所存放物品button的父物体位置

    public GameObject bagItem;//格子的物品
    public bool isChange = false;

    List<Image> slots;//格子的图片

    private void Awake()
    {
        //初始化pages
        List<Transform> ItemList = new List<Transform>();
        for (int i = 0; i < bagList.Length; i++)
        {
            for (int j = 0; j < bagList[i].childCount; j++)
            {
                ItemList.Add(bagList[i].GetChild(j));
            }
        }
        pages = ItemList.ToArray();
    }

    void Start()
    {
        Debug.Log("startstart");
        if(slots==null)
        {
            slots = new List<Image>();
            for(int page=0;page<this.pages.Length;page++)
            {
                slots.AddRange(this.pages[page].GetComponentsInChildren<Image>(true)); 
            }
        }
        StartCoroutine(InitBags());
    }

    IEnumerator InitBags()
    {
        Debug.Log("InitBags");

        this.gold.text = ItemManager.Instance.gold.ToString();

        int i;
        int n;
        //因存储方式的不同，BagManager.Instance.Items每一个格都不为空
        for (i=0,n=0;i<BagManager.Instance.Items.Length;i++)
        {//初始化背包道具页
            var item = BagManager.Instance.Items[n];
            n++;

            if(BagManager.Instance.bagObj.ContainsKey(i))
            {
                Destroy(BagManager.Instance.bagObj[i]);
                BagManager.Instance.bagObj.Remove(i);
            }
            //有物体则生成实体,没有则隐藏格子
            if(item.ItemId>0)
            {
                slots[i].gameObject.SetActive(true);
                GameObject go = Instantiate(bagItem, slots[i].transform);
                BagManager.Instance.bagObj.Add(i, go);
                UIBagItem uIBagItem=go.GetComponent<UIBagItem>();
                uIBagItem.id = item.ItemId;//获取id
                uIBagItem.num = item.Count;//获取数量
                uIBagItem.listId = n - 1;//获取在实体管理列表中的id
                uIBagItem.type = DataManager.Instance.Items[item.ItemId].Type;//获取物品类型
                uIBagItem.LoadMessag();
            }
            else
            {
                slots[i].gameObject.SetActive(false); 
            }
        }

        if(EquipmentManager.Instance.Equipments!=null)
        for(n=0;n< EquipmentManager.Instance.Equipments.Length; i++)
        {//初始化背包装备页
            if (i >= slots.Count) break;//超出部分不处理

            var item = EquipmentManager.Instance.Equipments[n];
                if (item == null) break;
            n++;

            if (EquipmentManager.Instance.equipObj.ContainsKey(i))
            {
                Destroy(EquipmentManager.Instance.equipObj[i]);
                EquipmentManager.Instance.equipObj.Remove(i);
            }
            //有物体则生成实体,没有则隐藏格子
            if (item.id > 0)
            {
                slots[i].gameObject.SetActive(true);
                GameObject go = Instantiate(bagItem, slots[i].transform);
                EquipmentManager.Instance.equipObj.Add(i, go);
                UIBagItem uIBagItem = go.GetComponent<UIBagItem>();
                uIBagItem.id = item.id;//获取id
                uIBagItem.num = 1;//获取数量
                uIBagItem.listId = n - 1;//获取在实体管理列表中的id
                uIBagItem.type = item.Define.Type;//获取物品类型
                uIBagItem.LoadMessag();
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }

        //将装备栏空着的位置隐藏
        for (; i < slots.Count; i++) slots[i].gameObject.SetActive(false);

        this.isChange = false;
        yield return null;
    }

    public void SetTitle(string title)
    {
        this.gold.text = User.Instance.CurrentCharacter.Id.ToString();
    }

    public void SetDescribe(string name,string type,string describe)
    {
        this.describeBar.SetActive(true);
        this.TName.text = name;
        this.TType.text = type;
        this.TDescribe.text = describe;
    }

    public void OnReset()
    {
        //如果背包处于打开状态，则直接更新，否则等待下一次打开时更新
        if (this.gameObject.activeSelf)
            this.ChangeBag();
        else
            this.isChange = true;
    }

    public void ChangeBag()
    {
        StartCoroutine(InitBags());
    }
}
