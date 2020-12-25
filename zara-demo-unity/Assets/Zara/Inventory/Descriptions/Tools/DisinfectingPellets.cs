namespace ZaraEngine.Inventory
{
    public class DisinfectingPellets : InventoryToolItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonTools.DisinfectingPellets; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 6.9f; }
        }
    }
}
