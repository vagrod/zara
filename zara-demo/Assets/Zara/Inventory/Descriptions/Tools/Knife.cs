namespace ZaraEngine.Inventory
{
    public class Knife : InventoryInfiniteHandheldToolItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonTools.Knife; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 138f; }
        }
    }
}
