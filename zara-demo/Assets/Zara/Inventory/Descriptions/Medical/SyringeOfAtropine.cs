namespace ZaraEngine.Inventory {
    public class SyringeOfAtropine : InventoryMedicalItemBase {
        public override string Name {
            get { return InventoryController.MedicalItems.AtropineSyringe; }
        }

        public override MedicineKinds MedicineKind {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit {
            get { return 11.6f; }
        }
    }
}