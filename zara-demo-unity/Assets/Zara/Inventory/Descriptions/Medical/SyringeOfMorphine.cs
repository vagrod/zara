namespace ZaraEngine.Inventory {
    public class SyringeOfMorphine : InventoryMedicalItemBase {
        public override string Name {
            get { return InventoryController.MedicalItems.MorphineSyringe; }
        }

        public override MedicineKinds MedicineKind {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit {
            get { return 13.7f; }
        }
    }
}