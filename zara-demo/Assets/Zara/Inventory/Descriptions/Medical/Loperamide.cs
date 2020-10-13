namespace ZaraEngine.Inventory
{
    public class Loperamide : InventoryMedicalItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.Loperamide; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Consumable; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 1.34f; }
        }
    }
}
