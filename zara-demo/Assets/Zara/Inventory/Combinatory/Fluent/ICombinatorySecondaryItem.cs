using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory.Combinatory.Fluent
{
    public interface ICombinatorySecondaryItem
    {

        ICombinatorySecondaryItem Plus<I>();
        ICombinatorySecondaryItem Plus<I>(int count);

        ItemsCombination And<I>();
        ItemsCombination And<I>(int count);

        ISpecialActionAvailability WithSpecialAction(ItemsCombination.SpecialActions action);
        ItemsCombination WithValidation(Func<List<IInventoryItem>, IGameController, bool> availabilityFunc);

    }
}
