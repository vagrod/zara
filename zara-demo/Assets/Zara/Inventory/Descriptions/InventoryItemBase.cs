using System;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Inventory
{
    public abstract class InventoryItemBase : IInventoryItem, IAcceptsStateChange
    {

        public Guid Id { get; } = Guid.NewGuid();

        public int Count { get; set; }

        public abstract string Name { get; }

        public abstract InventoryController.InventoryItemType[] Type { get; }

        public abstract float WeightGrammsPerUnit { get; }

        #region State Manage

        public virtual IStateSnippet GetState()
        {
            return new InventoryItemSnippet
            {
                Id = this.Id,
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
