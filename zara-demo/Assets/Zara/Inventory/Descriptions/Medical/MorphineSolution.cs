namespace ZaraEngine.Inventory
{
    public class MorphineSolution : InventoryMedicalItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.MorphineSolution; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Combinatory; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 11.5f; }
        }
    }
}
