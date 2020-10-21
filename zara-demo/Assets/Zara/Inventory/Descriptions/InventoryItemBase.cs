using System;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Inventory
{
    public abstract class InventoryItemBase : IInventoryItem, IAcceptsStateChange
    {

        public Guid Id { get; } = Guid.NewGuid();

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
