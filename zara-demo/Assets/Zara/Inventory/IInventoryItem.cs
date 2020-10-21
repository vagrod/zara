using System;

namespace ZaraEngine.Inventory
{
    public interface IInventoryItem
    {

        Guid Id { get; }
        string Name { get; }
        int Count { get; set; }
        InventoryController.InventoryItemType[] Type { get; }
        float WeightGrammsPerUnit { get; }

    }
}
