using Assets.Scripts.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Services
{
    class ItemService : Singleton<ItemService>,IDisposable
    {
        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnItemBuy);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
        }

        public void SendItemBuy(int shopId,int shopItemId,int buyNum)
        {
            Debug.LogFormat("SendItemBuy::shopId :{0} shopItemId:{1}", shopId, shopItemId);
            NetMessage message = new NetMessage();//new一个网络消息
            message.Request = new NetMessageRequest();//new一个网络消息的请求消息
            message.Request.itemBuy = new ItemBuyRequest();
            message.Request.itemBuy.shopId = shopId;
            message.Request.itemBuy.shopItemId = shopItemId;
            message.Request.itemBuy.buyNum = buyNum;

            NetClient.Instance.SendMessage(message);
        }

        private void OnItemBuy(object sender, ItemBuyResponse message)
        {
            Debug.Log("OnItemBuy");
            MessageBox.Show("购买成功");
        }
    }
}
