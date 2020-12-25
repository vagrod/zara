using System.Linq;

namespace ZaraEngine.Inventory
{
    public class InventorySpecialActionsProcessor
    {

        private readonly IGameController _gc;

        public InventorySpecialActionsProcessor(IGameController gc)
        {
            _gc = gc;
        }

        public CombinationSpecialActionResult ProcessAction(ItemsCombination combination)
        {
            if (combination.SpecialAction == ItemsCombination.SpecialActions.DisinfectWater)
            {
                return ProcessDisinfectWaterAction(combination);
            }
            
            return new CombinationSpecialActionResult
            {
                WasteInfo = null,
                IsAllowed = false,
                IsNoChanges = true 
            };
        }

         private CombinationSpecialActionResult ProcessDisinfectWaterAction(ItemsCombination combination)
        {
            var cmbWater = combination.ItemsNeeded.FirstOrDefault(y => y.ItemType.IsSubclassOf(typeof(WaterVesselItemBase))); 
            var cmbPellets = combination.ItemsNeeded.FirstOrDefault(y => y.ItemType.Name == typeof(DisinfectingPellets).Name);

            if (cmbWater == null || cmbPellets == null)
                return NoChanges();

            var invWater = (WaterVesselItemBase)_gc.Inventory.Items.FirstOrDefault(x => x.GetType().Name == cmbWater.ItemType.Name);
            var invPellets = (DisinfectingPellets)_gc.Inventory.Items.FirstOrDefault(x => x.GetType().Name == cmbPellets.ItemType.Name);

            if (invWater == null || invPellets == null)
                return NoChanges();

            if (invPellets.Count <= 0)
                return InsufficientResources();

            if (invWater.IsSafe)
                return NoChanges();

            invWater.Disinfect(_gc.WorldTime.Value);

            var wasteInfo =_gc.Inventory.WasteItem(invPellets, 1, checkOnly: false);
            
            return Applied(wasteInfo);
        }

        #region Helper Methods

        private CombinationSpecialActionResult NoChanges()
        {
            return new CombinationSpecialActionResult
            {
                IsAllowed = true,
                IsNoChanges = true,
                WasteInfo = new WasteCheckInfo { Result = WasteCheckInfo.WasteResult.Allowed }
            };
        }

        private CombinationSpecialActionResult InsufficientResources()
        {
            return new CombinationSpecialActionResult
            {
                IsAllowed = true,
                IsNoChanges = true,
                WasteInfo = new WasteCheckInfo { Result = WasteCheckInfo.WasteResult.InsufficientResources }
            };
        }

        private CombinationSpecialActionResult Applied(WasteCheckInfo wasteInfo)
        {
            return new CombinationSpecialActionResult
            {
                IsAllowed = true,
                IsNoChanges = false,
                WasteInfo = wasteInfo
            };
        }

        #endregion

        public class CombinationSpecialActionResult
        {
            public WasteCheckInfo WasteInfo { get; set; }
            public bool IsAllowed { get; set; }
            public bool IsNoChanges { get; set; }
        }

    }
}
