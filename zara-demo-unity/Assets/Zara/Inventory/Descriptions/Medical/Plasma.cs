namespace ZaraEngine.Inventory {
    public class Plasma : InventoryMedicalItemBase {
        public override string Name {
            get { return InventoryController.MedicalItems.Plasma; }
        }

        public override MedicineKinds MedicineKind {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit {
            get { return 1320f; }
        }
    }
}