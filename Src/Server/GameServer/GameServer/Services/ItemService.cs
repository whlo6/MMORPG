using Common;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;
using Common.Data;

namespace GameServer.Services
{
    class ItemService : Singleton<ItemService>
    {
        public ItemService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
        }

        public void Init()
        {
            
        }

        private void OnItemBuy(NetConnection<NetSession> sender, ItemBuyRequest mes)
        {
            Log.InfoFormat("OnItemBuy::shopId:{0}，shopItemId{1}", mes.shopId, mes.shopItemId);

            Character character = CharacterManager.Instance.Characters[sender.Session.Character.Id];
            Dictionary<int, ShopItemsDefine> shopItem = DataManager.Instance.ShopItems[mes.shopId];
            character.Info.Gold = character.Data.Gold;

            switch (mes.shopId)
            {
                case 1:
                    character.itemManager.AddItem(shopItem[mes.shopItemId].ID, mes.buyNum);
                    break;
                case 2:
                    character.equipmentManager.AddItem(shopItem[mes.shopItemId].ID, mes.buyNum);
                    break;
            }

            DBService.Instance.Save();

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.itemBuy = new ItemBuyResponse();
            message.Response.itemBuy.Result = Result.Success;
            message.Response.itemBuy.Errormsg = "None";

            byte[] data = PackageHandler.PackMessage(message);
            sender.Session.Character = character;
            sender.SendData(data, 0, data.Length);
        }
    }
}
