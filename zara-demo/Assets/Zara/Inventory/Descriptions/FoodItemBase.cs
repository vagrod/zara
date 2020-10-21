using System;
using System.Collections.Generic;
using System.Linq;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Inventory
{
    public abstract class FoodItemBase : InventoryConsumableItemBase
    {

        protected FoodItemBase()
        {
            FoodItemsGatheringInfo = new List<Tuple<DateTime, int>>();
        }

        public const string SpoiledPostfix = "$Spoiled";

        public override string Name
        {
            get
            {
                if (IsSpoiled)
                    return OriginalName + SpoiledPostfix;

                return OriginalName;
            }
        }

        public virtual string OriginalName
        {
            get { return null; }
        }

        private List<Tuple<DateTime, int>> FoodItemsGatheringInfo { get; set; }

        public virtual float FoodValue
        {
            get { return 0f; }
        }

        public virtual float WaterValue
        {
            get { return 0f; }
        }

        public int GetCountSpoiled(DateTime currentTime)
        {
            return FoodItemsGatheringInfo.Where(x => x.Item1 <= currentTime.AddMinutes(-MinutesUntilSpoiled)).Sum(x => x.Item2);
        }

        public int GetCountNormal(DateTime currentTime)
        {
            return FoodItemsGatheringInfo.Where(x => x.Item1 > currentTime.AddMinutes(-MinutesUntilSpoiled)).Sum(x => x.Item2);
        }

        public void AddGatheringInfo(DateTime time, int count)
        {
            FoodItemsGatheringInfo.Add(new Tuple<DateTime, int>(time, count));
        }

        public bool IsSpoiled { get; set; }

        public virtual int MinutesUntilSpoiled
        {
            get { return -1; }
        }

        public int TakeOneFromSpoiledGroup(DateTime currentTime)
        {
            var spoiledItem = FoodItemsGatheringInfo.FirstOrDefault(x => x.Item1 <= currentTime.AddMinutes(-MinutesUntilSpoiled));

            if (spoiledItem != null)
            {
                spoiledItem.Item2--;

                if (spoiledItem.Item2 <= 0)
                    FoodItemsGatheringInfo.Remove(spoiledItem);

                return GetCountSpoiled(currentTime);
            }

            return -1;
        }

        public int TakeOneFromNormalGroup(DateTime currentTime)
        {
            var normalItem = FoodItemsGatheringInfo.FirstOrDefault(x => x.Item1 > currentTime.AddMinutes(-MinutesUntilSpoiled));

            if (normalItem != null)
            {
                normalItem.Item2--;

                if (normalItem.Item2 <= 0)
                    FoodItemsGatheringInfo.Remove(normalItem);

                return GetCountNormal(currentTime);
            }

            return -1;
        }

        public void RemoveAllSpoiled(DateTime currentTime)
        {
            var copy = FoodItemsGatheringInfo.ToList();

            foreach (var x in copy)
            {
                if (x.Item1 <= currentTime.AddMinutes(-MinutesUntilSpoiled))
                    FoodItemsGatheringInfo.Remove(x);
            }
        }

        public void RemoveAllNormal(DateTime currentTime)
        {
            var copy = FoodItemsGatheringInfo.ToList();

            foreach (var x in copy)
            {
                if (x.Item1 > currentTime.AddMinutes(-MinutesUntilSpoiled))
                    FoodItemsGatheringInfo.Remove(x);
            }
        }

        #region State Manage

        public override IStateSnippet GetState()
        {
            return new InventoryFoodItemSnippet
            {
                Id = this.Id,
                Count = this.Count,
                ItemType = this.GetType(),
                IsSpoiled = this.IsSpoiled,
                FoodItemsGatheringInfo = this.FoodItemsGatheringInfo
            };
        }

        public override void RestoreState(IStateSnippet savedState)
        {
            var state = (InventoryFoodItemSnippet)savedState;

            Count = state.Count;
            IsSpoiled = state.IsSpoiled;
            FoodItemsGatheringInfo = state.FoodItemsGatheringInfo;
        }

        #endregion
    }
}
