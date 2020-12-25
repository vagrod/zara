namespace ZaraEngine.Inventory
{
    public class AntisepticSponge : InventoryMedicalItemBase
    {
        public override string Name
        {
            get { return InventoryController.MedicalItems.AntisepticSponge; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 3f; }
        }
    }
}
