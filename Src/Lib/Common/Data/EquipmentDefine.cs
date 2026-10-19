using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class EquipmentDefine
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Number { get; set; }
        public string ProName { get; set; }
        public int ATK { get; set; }
        public int STA { get; set; }
        public int WIT { get; set; }
        public int SellPrice { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public string Category { get; set; }//类别
        public bool CanUse { get; set; }
        public List<int> Param { get; set; }
    }
}
