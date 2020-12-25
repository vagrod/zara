namespace ZaraEngine.Inventory
{
    public class NeedleAndThread : InventoryInfiniteMedicalToolItemBase
    {
        public override string Name
        {
            get { return InventoryController.CommonTools.NeedleAndThread; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 67.4f; }
        }
    }
}