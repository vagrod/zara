namespace ZaraEngine.Inventory
{
    public abstract class InventoryHandheldItemBase : InventoryItemBase
    {
        public override InventoryController.InventoryItemType[] Type
        {
            get { return new[] { InventoryController.InventoryItemType.Handheld }; }
        }
    }
}
