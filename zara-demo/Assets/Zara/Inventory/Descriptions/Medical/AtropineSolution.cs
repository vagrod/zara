namespace ZaraEngine.Inventory
{
    public class AtropineSolution : InventoryMedicalItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.AtropineSolution; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Combinatory; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 9.4f; }
        }
    }
}
