namespace ZaraEngine.Inventory
{
    public class AntiVenomSolution : InventoryMedicalItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.AntiVenomSolution; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Combinatory; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 12.1f; }
        }
    }
}
