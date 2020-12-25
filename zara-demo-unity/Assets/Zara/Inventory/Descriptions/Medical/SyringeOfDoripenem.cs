namespace ZaraEngine.Inventory
{
    public class SyringeOfDoripenem : InventoryMedicalItemBase
    {

        public override string Name
        {
            get { return InventoryController.MedicalItems.DoripenemSyringe; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 18.3f; }
        }

    }
}