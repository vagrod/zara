using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class InventoryControllerStateContract {

        public float RoughWeight;

        public InventoryItemContract[] GenericInventoryItems;
        public FoodItemContract[] FoodInventoryItems;
        public WaterVesselItemContract[] WaterInventoryItems;

    }

}