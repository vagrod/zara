using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public abstract class InventoryHandheldItemBase : InventoryItemBase
    {
        public override InventoryController.InventoryItemType[] Type
        {
            get { return new[] { InventoryController.InventoryItemType.Handheld }; }
        }
    }
}
