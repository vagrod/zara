using System;

namespace ZaraEngine.StateManaging
{

    [Serializable]
    public class InventoryItemContract
    {

        public string ItemType;
        public int Count;

    }

    [Serializable]
    public class FoodItemContract
    {

        public string ItemType;
        public int Count;
        public bool IsSpoiled;
        public FoodItemGatheringInfoContract[] FoodItemsGatheringInfo;

    }

    [Serializable]
    public class WaterVesselItemContract
    {

        public string ItemType;
        public int Count;
        public int DosesLeft;
        public bool IsSafe;
        public DateTimeContract LastFillTime;
        public DateTimeContract LastDisinfectTime;
        public DateTimeContract LastBoilTime;

    }

    [Serializable]
    public class FoodItemGatheringInfoContract
    {
        public DateTimeContract TimeGathered;
        public int Count;

        public FoodItemGatheringInfoContract(Tuple<DateTime, int> x)
        {
            TimeGathered = new DateTimeContract(x.Item1);
            Count = x.Item2;
        }
    }

}
