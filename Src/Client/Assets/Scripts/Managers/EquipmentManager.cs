using Assets.Scripts.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    class EquipmentManager:Singleton<EquipmentManager>
    {
        public Equipment[] Equipments;//存放已拥有的装备
        public Dictionary<int, GameObject> equipObj = new Dictionary<int, GameObject>();
        public Dictionary<int, Sprite> Icon = new Dictionary<int, Sprite>();
        public int num;

        public int[] equipId = { -1, -1, -1, -1 };//已装备的物品的id

        internal void Init(List<NEquipmentInfo> equipments)
        {
            int t;//控制第几个类型的装备
            int n;//该类型的第几个装备
            int i;//存入的第几个装备
            Equipments = new Equipment[30];
            for (i=0,t=0;t< equipments.Count;t++)
            {
                for(n=0;n<equipments[t].Count && i<30;n++,i++)
                {
                    Equipments[i]=new Equipment(equipments[t]);

                    Debug.LogFormat("EquipmentManager:Init[{0}]", Equipments[i]);
                }
            }
            num = i;
            this.LoadItemIcon();
        }

        void LoadItemIcon()
        {
            //读取物品图片
            for (int i = 1; i <= DataManager.Instance.Equipments.Count; i++)
            {
                if (Icon.ContainsKey(i)) return;//如果存在则跳过
                Icon.Add(i, Resloader.Load<Sprite>(DataManager.Instance.Equipments[i].Icon));
            }
        }
    }
}
