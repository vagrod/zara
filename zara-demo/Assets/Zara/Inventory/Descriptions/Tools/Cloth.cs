namespace ZaraEngine.Inventory
{
    public class Cloth : InventoryToolItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonTools.Cloth; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 306.3f; }
        }
    }
}