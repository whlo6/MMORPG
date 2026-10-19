using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class Equipment
    {
        TEquipment dbItem;

        public int EquipmentID;
        public int Count;

        public Equipment(TEquipment item)
        {
            this.dbItem = item;
            this.EquipmentID = (short)item.Id;
            this.Count = (short)item.EquipCount;

        }

        public void Add(int count)
        {
            this.Count += count;
            dbItem.EquipCount = this.Count;
        }

        public void Remove(int count)
        {
            this.Count -= count;
            dbItem.EquipCount = this.Count;
        }

        public bool Use(int count = 1)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Format("ID:{0},Count:{1}", this.EquipmentID, this.Count);
        }
    }
}
