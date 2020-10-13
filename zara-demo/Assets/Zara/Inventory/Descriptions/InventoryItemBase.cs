using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public abstract class InventoryItemBase : IInventoryItem
    {
        public virtual string Name
        {
            get { return null; }
        }

        public int Count { get; set; }

        public virtual InventoryController.InventoryItemType[] Type
        {
            get { return new InventoryController.InventoryItemType[]{}; }
        }

        public virtual float WeightGrammsPerUnit
        {
            get { return -1f; }
        }
    }
}
