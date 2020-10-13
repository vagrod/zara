using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ZaraEngine.Inventory
{
    public interface IInventoryItem
    {

        string Name { get; }
        int Count { get; set; }
        InventoryController.InventoryItemType[] Type { get; }
        float WeightGrammsPerUnit { get; }

    }
}
