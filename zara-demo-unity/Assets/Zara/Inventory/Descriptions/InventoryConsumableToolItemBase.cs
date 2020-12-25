namespace ZaraEngine.Inventory
{
    public abstract class InventoryConsumableToolItemBase : InventoryConsumableItemBase
    {
        public override InventoryController.InventoryItemType[] Type
        {
            get { return new[] { InventoryController.InventoryItemType.Organic, InventoryController.InventoryItemType.Tool }; }
        }
    }
}