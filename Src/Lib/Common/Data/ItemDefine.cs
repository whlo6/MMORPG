using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    //public enum IteamType//物品类型（大体的）
    //{
    //    None=0,
    //    NORMAL,//正常的
    //    MATERIAL,//材料
    //}
    public enum ItemCategory//物品类别（具体的）
    {
        None=0,
        potion,//药水
        food,//食物
        expBook,//经验书
        badge,//徽章
        purse,//钱袋
        equipmentBox,//装备盒
        skillBook,//技能书
    }
    public enum ItemFunction
    {
        RecoverHP,
        RecoverMP,
        AddBuff,
        AddExp,
        AddMoney,
        AddIteam,
        AddSkillPoint,
    }

    public class ItemDefine
    {
        public int ID { get; set; }
        public int Level { get; set; }
        public string proName { get; set; }
        public int Price { get; set; }
        public int SellPrice { get; set; }
        public int StackLimit { get; set; }//堆叠限制
        public string Icon { get; set; }//道具图标
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public string Category { get; set; }
        public bool CanUse { get; set; }
        public float UseCd { get; set; }
        public ItemFunction Function { get; set; }
        public int Param { get; set; }
        public List<int> Params { get; set; }
    }
}
