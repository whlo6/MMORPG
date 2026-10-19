using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cnfirmBuy : UIWindow {

    public Text tip;
    public InputField TNum;

    public UIShopItem shopItem;
    public int id;
    public string itemName;
    public int price;
    public int inventory;
    public int num = 1;//购买数量

    public void OnClickChange(int n)
    {
        num = int.Parse(TNum.text);
        this.num += n;
        if (num < 1) num = 1;
        TNum.text = this.num.ToString();
        ChangeTip();
    }

    public void OnClickBuy()
    {
        num = int.Parse(TNum.text);
        if(num>inventory)
        {
            MessageBox.Show("超出库存");
            return;
        }
        if(num*price> ItemManager.Instance.gold)
        {
            MessageBox.Show("钱不够");
            return;
        }
        shopItem.message.Inventory -= num;//库存减少,dataManager内的数据也会减少
        ItemManager.Instance.gold -= num * price;//钱减少
        shopItem.UpdateItem();

        ItemService.Instance.SendItemBuy(ShopManager.Instance.shop.shopId, shopItem.shopListId, num);
        UpdateMyItems();
        OnCloseClick();
    }

    public void ChangeTip()
    {
        if (TNum.text == null||TNum.text=="")
            TNum.text = "1";
        num = int.Parse(TNum.text);
        tip.text = "购买" + itemName + "   数量：" + num + "   花费：" + num * price;
    }

    void UpdateMyItems()
    {
        int itemId = shopItem.message.ID;
        if(shopItem.type==ItemType.Equip)
        {
            int n = EquipmentManager.Instance.num;
            for(int i=0;i<num;i++)
            {
                if (n >= 30) break;
                int number = DataManager.Instance.Equipments[itemId].Number;
                NEquipmentInfo equipment = new NEquipmentInfo { Id = itemId, Number = number, Count = 1 };
                EquipmentManager.Instance.Equipments[n] = new Equipment(equipment);
                n++;
            }
            EquipmentManager.Instance.num = n;
        }
        else
        {
            if (ItemManager.Instance.Items.ContainsKey(itemId))
            {
                ItemManager.Instance.Items[itemId].Count += num;
            }
            else
            {
                NItemInfo item = new NItemInfo { Id = itemId, Count = num };
                ItemManager.Instance.Items.Add(itemId, new Item(item));
            }
        }
        BagManager.Instance.Reset();//更新背包管理
        if (BagManager.Instance.uiBag!=null)//如果背包窗口已经创建，则更新背包窗口
            BagManager.Instance.uiBag.OnReset();
    }
}
