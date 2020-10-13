using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public class WaterproofPants : ClothesItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonClothes.WaterproofPants; }
        }

        public override ClothesTypes ClothesType
        {
            get { return ClothesTypes.Pants; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 1069; }
        }

        public override int WaterResistance
        {
            get { return 20; }
        }

        public override int ColdResistance
        {
            get { return 6; }
        }

        public override int Order
        {
            get { return 1; }
        }
    }
}
