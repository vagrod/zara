namespace ZaraEngine.Inventory
{
    public class EmptySyringe : InventoryMedicalItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.EmptySyringe; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Combinatory; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 2.2f; }
        }
    }
}
