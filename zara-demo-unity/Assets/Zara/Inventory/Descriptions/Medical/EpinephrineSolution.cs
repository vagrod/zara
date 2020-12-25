namespace ZaraEngine.Inventory
{
    public class EpinephrineSolution : InventoryMedicalItemBase
    {

        public override string Name
        {
            get { return InventoryController.MedicalItems.EpinephrineSolution; }
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
