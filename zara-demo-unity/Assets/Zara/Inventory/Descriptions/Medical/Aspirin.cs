namespace ZaraEngine.Inventory
{
    public class Aspirin : InventoryMedicalItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.Aspirin; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Consumable; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 1.1f; }
        }
    }
}
