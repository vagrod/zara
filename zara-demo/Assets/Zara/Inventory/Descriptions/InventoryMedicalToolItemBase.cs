using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public abstract class InventoryMedicalToolItemBase : InventoryMedicalItemBase
    {

        public override InventoryController.InventoryItemType[] Type
        {
            get { return new[] { InventoryController.InventoryItemType.Medical, InventoryController.InventoryItemType.Tool }; }
        }

    }
}
