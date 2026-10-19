using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    class ItemManager:Singleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        public Dictionary<int, Sprite> ItemIcon = new Dictionary<int, Sprite>();//物品图标
        public long gold;

        internal void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach(var info in items)
            {
                Item item = new Item(info);
                if (this.Items.ContainsKey(item.Id)) this.Items[item.Id].Count += item.Count;//id重复则累加
                else this.Items.Add(item.Id, item);

                Debug.LogFormat("ItemManager:Init[{0}]", item);
            }
            this.LoadItemIcon();
        }

        void LoadItemIcon()
        {
            //读取物品图片
            for (int i = 1; i <= DataManager.Instance.Items.Count; i++)
            {
                if (ItemIcon.ContainsKey(i)) return;//如果存在则跳过
                ItemIcon.Add(i, Resloader.Load<Sprite>(DataManager.Instance.Items[i].Icon));
            }
        }

        public ItemDefine GetItem(int itemId)
        {

            return null;
        }

        public bool UseItem(int itemId)
        { 
            return false;
        }
        public bool UseItem(ItemDefine item)
        {
            return false;
        }


    }
}
