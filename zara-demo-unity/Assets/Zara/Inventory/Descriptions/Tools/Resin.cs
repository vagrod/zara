namespace ZaraEngine.Inventory
{
    public class Resin : InventoryToolItemBase
    {
        public override string Name
        {
            get { return "Resin"; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 34f; }
        }
    }
}