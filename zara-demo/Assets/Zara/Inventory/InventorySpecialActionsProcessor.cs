using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return new CombinationSpecialActionResult
            {
                WasteInfo = null,
                IsAllowed = false,
                IsNoChanges = true 
            };
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
