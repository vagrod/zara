namespace ZaraEngine.Inventory
{
    public abstract class InventoryHandheldToolItemBase : InventoryHandheldItemBase
    {
        public override InventoryController.InventoryItemType[] Type
        {
            get { return new[] { InventoryController.InventoryItemType.Tool, InventoryController.InventoryItemType.Handheld }; }
        }
    }
}
