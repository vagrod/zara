namespace ZaraEngine.Inventory
{
    public abstract class InventoryToolItemBase : InventoryItemBase
    {

        public virtual bool IsEquippable { get; set; }

        public override InventoryController.InventoryItemType[] Type
        {
            get { return new[] { InventoryController.InventoryItemType.Tool }; }
        }
    }
}
