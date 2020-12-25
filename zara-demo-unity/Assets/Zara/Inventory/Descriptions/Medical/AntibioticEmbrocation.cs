namespace ZaraEngine.Inventory
{
    public class AntibioticEmbrocation : InventoryMedicalToolItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.AntibioticEmbrocation; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 79.1f; }
        }
    }
}