using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public class LeafHat : ClothesItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonClothes.LeafHat; }
        }

        public override ClothesTypes ClothesType
        {
            get { return ClothesTypes.Hat; }
        }

        public override int WaterResistance
        {
            get { return 10; }
        }

        public override int ColdResistance
        {
            get { return 1; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 213f; }
        }
    }
}
