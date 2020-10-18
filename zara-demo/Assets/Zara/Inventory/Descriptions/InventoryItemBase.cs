using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Inventory
{
    public abstract class InventoryItemBase : IInventoryItem, IAcceptsStateChange
    {
        public virtual string Name
        {
            get { return null; }
        }

        public int Count { get; set; }

        public virtual InventoryController.InventoryItemType[] Type
        {
            get { return new InventoryController.InventoryItemType[]{}; }
        }

        public virtual float WeightGrammsPerUnit
        {
            get { return -1f; }
        }

        #region State Manage

        public virtual IStateSnippet GetState()
        {
            return new InventoryItemSnippet
            {
                Count = this.Count,
                ItemType = this.GetType()
            };
        }

        public virtual void RestoreState(IStateSnippet savedState)
        {
            var state = (InventoryItemSnippet)savedState;

            Count = state.Count;
        }

        #endregion
    }
}
