namespace ZaraEngine.Inventory {
    public class Bandage : InventoryMedicalToolItemBase {
        public override string Name {
            get { return InventoryController.MedicalItems.Bandage; }
        }

        public override MedicineKinds MedicineKind {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit {
            get { return 38.6f; }
        }
    }
}