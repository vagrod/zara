using System;
using System.Collections.Generic;

namespace ZaraEngine.Inventory.Combinatory.Fluent
{
    public class CombinatoryItemBuilder<T> : ICombinatorySecondaryItem, ISpecialActionAvailability
    {

        private readonly ItemsCombination _obj;

        public CombinatoryItemBuilder(Type mainItem, Type firstItem, int count)
        {
            _obj = new ItemsCombination
            {
                ResultType = mainItem,
                ItemsNeeded = new List<ItemInSetInfo>(new[] {new ItemInSetInfo(firstItem, count)})
            };
        }

        public ISpecialActionAvailability WithSpecialAction(ItemsCombination.SpecialActions action)
        {
            _obj.SpecialAction = action;

            return this;
        }

        public ItemsCombination AvailableWhen(Func<List<IInventoryItem>, bool> availabilityFunc)
        {
            _obj.CheckForActionAvailability = items => availabilityFunc.Invoke(items) ? CheckResultAllowed() : CheckResultNotAllowed();

            return _obj;
        }

        public ItemsCombination WithValidation(Func<List<IInventoryItem>, IGameController, bool> validationFunc)
        {
            _obj.GetIsValidCombination = (items, gc) => validationFunc.Invoke(items, gc) ? CheckResultAllowed() : CheckResultNotAllowed();

            return _obj;
        }

        private InventoryCombinatoryResult CheckResultAllowed()
        {
            return new InventoryCombinatoryResult
            {
                IsViaSpecialAction = true,
                Result = InventoryCombinatoryResult.CombinatoryResult.Allowed
            };
        }

        private InventoryCombinatoryResult CheckResultNotAllowed()
        {
            return new InventoryCombinatoryResult
            {
                IsViaSpecialAction = true,
                Result = InventoryCombinatoryResult.CombinatoryResult.CombinationDoesNotExist
            };
        }

        public static ICombinatorySecondaryItem Is<I>()
        {
            return new CombinatoryItemBuilder<T>(typeof(T), typeof(I), 1);
        }

        public static ICombinatorySecondaryItem Is<I>(int count)
        {
            return new CombinatoryItemBuilder<T>(typeof(T), typeof(I), count);
        }

        public ICombinatorySecondaryItem Plus<I>()
        {
            _obj.ItemsNeeded.Add(new ItemInSetInfo(typeof(I)));

            return this;
        }

        public ICombinatorySecondaryItem Plus<I>(int count)
        {
            _obj.ItemsNeeded.Add(new ItemInSetInfo(typeof(I), count));

            return this;
        }

        public ItemsCombination And<I>()
        {
            _obj.ItemsNeeded.Add(new ItemInSetInfo(typeof(I)));

            return _obj;
        }

        public ItemsCombination And<I>(int count)
        {
            _obj.ItemsNeeded.Add(new ItemInSetInfo(typeof(I), count));

            return _obj;
        }
    }
}
