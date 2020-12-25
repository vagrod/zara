namespace ZaraEngine.Inventory
{
    public class DoripenemSolution : InventoryMedicalItemBase
    {

        public override string Name
        {
            get { return InventoryController.MedicalItems.DoripenemSolution; }
        }

        public override MedicineKinds MedicineKind
        {
            get { return MedicineKinds.Combinatory; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 11.6f; }
        }

    }
}
