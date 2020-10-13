using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public class WaterproofJacket : ClothesItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonClothes.WaterproofJacket; }
        }

        public override ClothesTypes ClothesType
        {
            get { return ClothesTypes.Jacket; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 2186; }
        }

        public override int WaterResistance
        {
            get { return 30; }
        }

        public override int ColdResistance
        {
            get { return 7; }
        }

        public override int Order
        {
            get { return 1; }
        }
    }
}
