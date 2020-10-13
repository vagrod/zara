using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public abstract class InventoryMedicalItemBase : InventoryConsumableItemBase
    {

        public enum MedicineKinds
        {
            Unknown,
            Consumable,
            Appliance,
            Combinatory
        }

        public virtual MedicineKinds MedicineKind {
            get { return MedicineKinds.Unknown; }
        }

        public override InventoryController.InventoryItemType[] Type
        {
            get { return new[] { InventoryController.InventoryItemType.Medical }; }
        }
    }
}
