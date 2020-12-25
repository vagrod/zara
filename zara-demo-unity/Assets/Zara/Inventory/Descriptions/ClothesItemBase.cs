namespace ZaraEngine.Inventory
{
    public abstract class ClothesItemBase : InventoryItemBase 
    {

        public enum ClothesTypes
        {
            Unknown,
            Hat,
            Mask,
            Jacket,
            Pants,
            Boots,
            Gloves
        }

        public virtual ClothesTypes ClothesType
        {
            get { return ClothesTypes.Unknown; }
        }

        public virtual int Order
        {
            get { return 0; }
        }

        public abstract int WaterResistance { get; }

        public abstract int ColdResistance { get; }

        public virtual bool CanBeTool()
        {
            return false;
        }

        public override InventoryController.InventoryItemType[] Type
        {
            get { return new[] { InventoryController.InventoryItemType.Clothes }; }
        }

    }
}
