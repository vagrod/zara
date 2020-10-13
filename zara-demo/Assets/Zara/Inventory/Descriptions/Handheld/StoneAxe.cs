namespace ZaraEngine.Inventory
{
    public class StoneAxe : InventoryHandheldItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonTools.StoneAxe; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 4381f; }
        }
    }
}