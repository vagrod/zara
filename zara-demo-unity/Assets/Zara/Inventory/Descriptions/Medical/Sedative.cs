namespace ZaraEngine.Inventory
{
    public class Sedative : InventoryMedicalItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.Sedative; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Consumable; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 1.76f; }
        }
    }
}
