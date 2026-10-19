using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    public class Equipment
    {
        public int id;
        public int number;
        public int count;
        public EquipmentDefine Define;

        public Equipment(NEquipmentInfo info)
        {
            this.id = info.Id;
            //this.number = info.Number;
            this.number = DataManager.Instance.Equipments[this.id].Number;
            this.count = 1;
            Define = DataManager.Instance.Equipments[this.id];
        }

        public Equipment()
        {

        }
    }
}
