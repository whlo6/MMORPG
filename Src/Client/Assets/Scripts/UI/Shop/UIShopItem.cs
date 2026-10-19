using Assets.Scripts.Managers;
using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour, IPointerClickHandler
{
    public int shopListId;//在商品列表中的Id
    public Image icon;
    public Text itemName;
    public Text inventory;
    public Text num;
    public Text price;
    public GameObject soldOut;
    public ItemType type;

    public ShopItemsDefine message;

    public void Init()
    {
        itemName.text = message.Name;
        inventory.text = "库存：" + message.Inventory.ToString();
        num.text = "X" + message.Num.ToString();
        price.text = message.Price.ToString();
        if (message.State == 0) this.soldOut.SetActive(true);
    }

    //点击商品时，显示详情
    public void OnPointerClick(PointerEventData eventData)
    {
        ShopManager.Instance.SetItemDescribe(message.Description);
    }

    //改变商品的可购买状态
    public void UpdateItem()
    {
        inventory.text = "库存：" + message.Inventory.ToString();//更新库存显示
        ShopManager.Instance.shop.gold.text = ItemManager.Instance.gold.ToString();//更新金币显示
        if (message.Inventory <= 0)
        {
            this.soldOut.SetActive(true);//库存为0时，显示售罄
            this.message.State = 0;//更新商品状态
        }
    }

    public void OnClickBuy()
    {
        cnfirmBuy confirm = UIManager.Instance.Show<cnfirmBuy>();
        confirm.num = 1;
        confirm.TNum.text = "1";
        confirm.shopItem = this;
        confirm.id = message.ID;
        confirm.itemName = message.Name;
        confirm.price = message.Price;
        confirm.inventory = message.Inventory;
        confirm.ChangeTip();
    }
}
