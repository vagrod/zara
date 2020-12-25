namespace ZaraEngine.Inventory
{
    public class Antibiotic : InventoryMedicalItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.Antibiotic; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Consumable; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 1.2f; }
        }
    }
}
