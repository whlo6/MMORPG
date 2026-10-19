using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Managers;
using Common.Data;

public class ShopManager : Singleton<ShopManager> {

    public UIShop shop;
    public Dictionary<int, GameObject> shopItem = new Dictionary<int, GameObject>();//商品
    public Dictionary<int, Sprite> shopOwnerIcon = new Dictionary<int, Sprite>();

    public void Init()
    {
        //读取人物图片
        for (int i = 1; i <= DataManager.Instance.Shop.Count; i++)
        {
            if (shopOwnerIcon.ContainsKey(i)) return;//如果存在则跳过
            shopOwnerIcon.Add(i, Resloader.Load<Sprite>(DataManager.Instance.Shop[i].Icon));
        }

        //注册商店打开事件
        NpcManager.Instance.RegisterNpcEvent(NpcFunction.InvokeShop, ShowShop);
    }

    bool ShowShop(NpcDefine npc)
    {
        shop = UIManager.Instance.Show<UIShop>();
        //当商店类型改变时，重新读取商品
        if (shop.shopId!=-1 && npc.Param != shop.shopId)
        {
            shop.shopId = npc.Param;
            shop.OnReset();
        }
        shop.shopId = npc.Param;

        return true;
    }

    //设置商品描述面板
    public void SetItemDescribe(string str)
    {
        if (shop == null) return;
        shop.SetItemDescribe(str);
    }
}
