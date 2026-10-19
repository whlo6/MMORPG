using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow {

    public int shopId = -1;
    public Text shopName;
    public Text shopDescribe;
    public Text gold;//金币
    public Image shopOwner;//商店看板娘
    public GameObject describeBar;//商品描述面板
    public Text itemDescribe;//商品描述
    
    public Transform[] pages;//格子的位置
    public Transform[] shopList;//商店所存放物品button的父物体位置

    public GameObject shopItem;//商店格子的物品

    List<Image> slots;//格子占位格图片

    private void Awake()
    {
        /***** pages依旧读取两个bag页，但后续只处理一个bag页 *****/
        //初始化pages
        List<Transform> ItemList = new List<Transform>();
        for (int i = 0; i < shopList.Length; i++)
        {
            for (int j = 0; j < shopList[i].childCount; j++)
            {
                ItemList.Add(shopList[i].GetChild(j));
            }
        }
        pages = ItemList.ToArray();
    }

    void Start()
    {
        if (slots == null)
        {
            slots = new List<Image>();
            for (int page = 0; page < this.pages.Length; page++)
            {
                slots.AddRange(this.pages[page].GetComponentsInChildren<Image>(true));
            }
        }
        if (this.shopId < 1)
        {
            Debug.Log("shopId is Null");
            return;
        }
        StartCoroutine(InitShops());
    }

    IEnumerator InitShops()
    {
        Debug.Log("InitShops");

        //更新商店基础信息
        shopName.text = DataManager.Instance.Shop[shopId].Name.ToString();
        shopDescribe.text = DataManager.Instance.Shop[shopId].Describe.ToString();
        shopOwner.sprite = ShopManager.Instance.shopOwnerIcon[shopId];
        this.gold.text = ItemManager.Instance.gold.ToString();

        int i;//表示第几个格子

        for (i=0; i < DataManager.Instance.ShopItems[shopId].Count; i++)
        {
            var item = DataManager.Instance.ShopItems[shopId][i+1];

            //如果该位置已经有实体，则删除
            if(ShopManager.Instance.shopItem.ContainsKey(i))
            {
                GameObject.Destroy(ShopManager.Instance.shopItem[i]);
                ShopManager.Instance.shopItem.Remove(i);
            }
            //有物体则生成实体,没有则隐藏格子
            if (item.ID > 0)
            {
                slots[i].gameObject.SetActive(true);
                GameObject obj = Instantiate(shopItem, slots[i].transform);
                ShopManager.Instance.shopItem.Add(i, obj);
                UIShopItem uIShopItem = obj.GetComponent<UIShopItem>();
                uIShopItem.shopListId = i + 1;
                switch (shopId)
                {
                    case 1:
                        uIShopItem.type = DataManager.Instance.Items[item.ID].Type;
                        uIShopItem.icon.sprite = ItemManager.Instance.ItemIcon[item.ID];
                        break;
                    case 2:
                        uIShopItem.type = DataManager.Instance.Equipments[item.ID].Type;
                        uIShopItem.icon.sprite = EquipmentManager.Instance.Icon[item.ID];
                        break;
                }
                uIShopItem.message = item;//获取信息
                uIShopItem.Init();
            }
            else
            {
                Debug.LogFormat("Item Id Is Null//shopId:{0}--item:{1}", shopId, i + 1);
            }
        }
        //没有商品的格子，隐藏
        for(;i<slots.Count;i++)
            slots[i].gameObject.SetActive(false);

        yield return null;
    }

    //设置描述面板
    public void SetItemDescribe(string str)
    {
        this.describeBar.SetActive(true);
        this.itemDescribe.text = str;
    }

    public void OnReset()
    {
        StartCoroutine(InitShops());
    }
}
