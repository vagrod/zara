namespace ZaraEngine.Inventory
{
    public class Pin : InventoryToolItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonTools.Pin; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 1.7f; }
        }
    }
}