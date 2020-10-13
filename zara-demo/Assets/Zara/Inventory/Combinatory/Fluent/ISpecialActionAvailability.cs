using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory.Combinatory.Fluent
{
    public interface ISpecialActionAvailability
    {

        ItemsCombination AvailableWhen(Func<List<IInventoryItem>, bool> availabilityFunc);
    }
}
