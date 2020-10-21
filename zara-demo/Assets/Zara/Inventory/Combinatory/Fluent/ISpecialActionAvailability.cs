using System;
using System.Collections.Generic;

namespace ZaraEngine.Inventory.Combinatory.Fluent
{
    public interface ISpecialActionAvailability
    {

        ItemsCombination AvailableWhen(Func<List<IInventoryItem>, bool> availabilityFunc);
    }
}
