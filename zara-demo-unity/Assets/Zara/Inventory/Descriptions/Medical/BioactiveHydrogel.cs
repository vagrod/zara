namespace ZaraEngine.Inventory {
    public class BioactiveHydrogel : InventoryMedicalItemBase {
        public override string Name {
            get { return InventoryController.MedicalItems.BioactiveHydrogel; }
        }

        public override MedicineKinds MedicineKind {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit {
            get { return 318.7f; }
        }
    }
}