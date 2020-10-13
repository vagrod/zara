using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public class RubberBoots : ClothesItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonClothes.RubberBoots; }
        }

        public override ClothesTypes ClothesType
        {
            get { return ClothesTypes.Boots; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 1615; }
        }

        public override int WaterResistance
        {
            get { return 10; }
        }

        public override int ColdResistance
        {
            get { return 3; }
        }

        public override int Order
        {
            get { return 2; }
        }
    }
}
