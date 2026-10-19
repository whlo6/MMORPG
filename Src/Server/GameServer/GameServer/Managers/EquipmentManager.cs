using Common;
using GameServer.Entities;
using GameServer.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class EquipmentManager
    {
        Character Owner;

        public Dictionary<int, Equipment> Items = new Dictionary<int, Equipment>();

        public EquipmentManager(Character owner)
        {
            this.Owner = owner;

            foreach (var item in owner.Data.Equipments)
            {
                this.Items.Add(item.Id, new Equipment(item));
            }
        }

        public bool UseItem(int itemId, int count = 1)
        {
            Log.InfoFormat("[{0}]UseItem[{1}:{2}]", this.Owner.Data.ID, itemId, count);
            Equipment item = null;

            if (this.Items.TryGetValue(itemId, out item))
            {
                if (item.Count < count)
                {
                    return false;
                }

                item.Remove(count);

                return true;
            }
            return false;
        }

        public bool HasItem(int itemId)//判断道具是否存在
        {
            Equipment item = null;
            if (this.Items.TryGetValue(itemId, out item))
                return item.Count > 0;
            return false;
        }

        public Equipment GetItem(int itemId)
        {
            Equipment item = null;
            this.Items.TryGetValue(itemId, out item);
            Log.InfoFormat("[{0}]GetItem[{1}:{2}]", this.Owner.Data.ID, itemId, item);
            return item;
        }

        public bool AddItem(int itemId, int count)
        {
            Equipment item = null;

            if (this.Items.TryGetValue(itemId, out item))
            {
                item.Add(count);
            }
            else
            {
                TEquipment dbitem = new TEquipment();
                dbitem.CharacterID = Owner.Data.ID;
                dbitem.Owner = Owner.Data;
                dbitem.EquipID = itemId;
                dbitem.EquipCount = count;
                Owner.Data.Equipments.Add(dbitem);
                item = new Equipment(dbitem);
                this.Items.Add(itemId, item);
            }
            Log.InfoFormat("[{0}]AddItem[{1}] addCount:{2}", this.Owner.Data.ID, item, count);
            //DBService.Instance.Save();
            return true;
        }

        public bool RemoveItem(int ItemId, int count)
        {
            if (!this.Items.ContainsKey(ItemId))
            {
                return false;
            }

            Equipment item = this.Items[ItemId];
            if (item.Count < count)
                return false;
            item.Remove(count);
            Log.InfoFormat("[{0}]RemoveItem[{1}] removeCount{2}", this.Owner.Data.ID, item, count);
            //DBService.Instance.Save();
            return true;
        }

        public void GetItemInfos(List<NEquipmentInfo> list)
        {
            foreach (var item in this.Items)
            {
                list.Add(new NEquipmentInfo() { Id = item.Value.EquipmentID, Count = item.Value.Count });
            }
        }
    }
}
