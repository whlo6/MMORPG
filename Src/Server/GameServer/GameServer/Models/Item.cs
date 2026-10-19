using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Item
    {
        TCharacterIteam dbItem;

        public int ItemID;
        public int Count;

        public Item(TCharacterIteam item)
        {
            this.dbItem = item;
            this.ItemID = (short)item.IteamID;
            this.Count = (short)item.IteamCount;
            
        }

        public void Add(int count)
        {
            this.Count += count;
            dbItem.IteamCount = this.Count;
        }

        public void Remove(int count)
        {
            this.Count -= count;
            dbItem.IteamCount = this.Count;
        }

        public bool Use(int count=1)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Format("ID:{0},Count:{1}", this.ItemID, this.Count);
        }
    }
}
