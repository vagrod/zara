namespace ZaraEngine.Inventory
{
    public class Acetaminophen : InventoryMedicalItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.Acetaminophen; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Consumable; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 1.9f; }
        }
    }
}
