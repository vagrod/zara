using System;
using System.Collections.Generic;
using System.Linq;

namespace ZaraEngine.StateManaging
{

    public class InventoryItemSnippet : SnippetBase
    {

        public InventoryItemSnippet() : base() { }
        public InventoryItemSnippet(object contract) : base(contract) { }

        #region Data Fields

        public Type ItemType { get; set; }
        public int Count { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new InventoryItemContract()
            {
                ItemType = this.ItemType.FullName,
                Count = this.Count
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (InventoryItemContract)o;

            ItemType = Type.GetType(c.ItemType);
            Count = c.Count;

            ChildStates.Clear();
        }

    }

    public class InventoryFoodItemSnippet : InventoryItemSnippet
    {

        public InventoryFoodItemSnippet() : base() { }
        public InventoryFoodItemSnippet(object contract) : base(contract) { }

        #region Data Fields

        public bool IsSpoiled { get; set; }
        public List<Tuple<DateTime, int>> FoodItemsGatheringInfo { get; set; } = new List<Tuple<DateTime, int>>();

        #endregion 

        public override object ToContract()
        {
            var c = new FoodItemContract()
            {
                ItemType = this.ItemType.FullName,
                Count = this.Count,
                IsSpoiled = this.IsSpoiled,
                FoodItemsGatheringInfo = FoodItemsGatheringInfo.ConvertAll(x => new FoodItemGatheringInfoContract(x)).ToArray()
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (FoodItemContract)o;

            ItemType = Type.GetType(c.ItemType);
            Count = c.Count;
            IsSpoiled = c.IsSpoiled;
            FoodItemsGatheringInfo = c.FoodItemsGatheringInfo.ToList().ConvertAll(x => new Tuple<DateTime, int>(x.TimeGathered.ToDateTime(), x.Count));

            ChildStates.Clear();
        }

    }

    public class InventoryWaterVesselItemSnippet : InventoryItemSnippet
    {

        public InventoryWaterVesselItemSnippet() : base() { }
        public InventoryWaterVesselItemSnippet(object contract) : base(contract) { }

        #region Data Fields

        public int DosesLeft { get; set; }
        public bool IsSafe { get; set; }
        public DateTime? LastFillTime { get; set; }
        public DateTime? LastDisinfectTime { get; set; }
        public DateTime? LastBoilTime { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new WaterVesselItemContract()
            {
                ItemType = this.ItemType.FullName,
                Count = this.Count,
                DosesLeft = this.DosesLeft,
                IsSafe = this.IsSafe,
                LastFillTime = this.LastFillTime.HasValue ? new DateTimeContract(this.LastFillTime.Value) : null,
                LastDisinfectTime = this.LastDisinfectTime.HasValue ? new DateTimeContract(this.LastDisinfectTime.Value) : null,
                LastBoilTime = this.LastBoilTime.HasValue ? new DateTimeContract(this.LastBoilTime.Value) : null
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (WaterVesselItemContract)o;

            ItemType = Type.GetType(c.ItemType);
            Count = c.Count;
            DosesLeft = c.DosesLeft;
            IsSafe = c.IsSafe;
            LastFillTime = c.LastFillTime == null || c.LastFillTime.IsEmpty ? (DateTime?)null : c.LastFillTime.ToDateTime();
            LastDisinfectTime = c.LastDisinfectTime == null || c.LastDisinfectTime.IsEmpty ? (DateTime?)null : c.LastDisinfectTime.ToDateTime();
            LastBoilTime = c.LastBoilTime == null || c.LastBoilTime.IsEmpty ? (DateTime?)null : c.LastBoilTime.ToDateTime();

            ChildStates.Clear();
        }

    }

}
