namespace ZaraEngine.Inventory
{
    public class Rope : InventoryToolItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonTools.Rope; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 1150f; }
        }
    }
}
