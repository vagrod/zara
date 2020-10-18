using System;
using System.Collections.Generic;
using System.Linq;

namespace ZaraEngine.StateManaging
{
    public class InventoryControllerStateSnippet : SnippetBase
    {

        public InventoryControllerStateSnippet() : base() { }
        public InventoryControllerStateSnippet(object contract) : base(contract) { }

        #region Data Fields

        public float RoughWeight { get; set; }

        public List<InventoryItemSnippet> GenericInventoryItems { get; set; } = new List<InventoryItemSnippet>();
        public List<InventoryFoodItemSnippet> FoodInventoryItems { get; set; } = new List<InventoryFoodItemSnippet>();
        public List<InventoryWaterVesselItemSnippet> WaterInventoryItems { get; set; } = new List<InventoryWaterVesselItemSnippet>();

        #endregion 

        public override object ToContract()
        {
            var c = new InventoryControllerStateContract
            {
                RoughWeight = this.RoughWeight,
                GenericInventoryItems = this.GenericInventoryItems.ConvertAll(x => (InventoryItemContract)x.ToContract()).ToArray(),
                FoodInventoryItems = this.FoodInventoryItems.ConvertAll(x => (FoodItemContract)x.ToContract()).ToArray(),
                WaterInventoryItems = this.WaterInventoryItems.ConvertAll(x => (WaterVesselItemContract)x.ToContract()).ToArray()
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (InventoryControllerStateContract)o;

            RoughWeight = c.RoughWeight;

            GenericInventoryItems = c.GenericInventoryItems.ToList().ConvertAll(x => new InventoryItemSnippet(x));
            FoodInventoryItems = c.FoodInventoryItems.ToList().ConvertAll(x => new InventoryFoodItemSnippet(x));
            WaterInventoryItems = c.WaterInventoryItems.ToList().ConvertAll(x => new InventoryWaterVesselItemSnippet(x));

            ChildStates.Clear();
        }

    }
}
