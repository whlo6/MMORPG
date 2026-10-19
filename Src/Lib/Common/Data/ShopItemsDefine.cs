using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class ShopItemsDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Inventory { get; set; }//库存
        public int Num { get; set; }//单次购买的数量
        public int Price { get; set; }
        public int State { get; set; }//状态
        public string Description { get; set; }
    }
}
