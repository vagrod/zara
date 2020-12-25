namespace ZaraEngine.Inventory.Descriptions.Medical {
    public class Splint : InventoryMedicalItemBase {
        public override string Name {
            get { return InventoryController.MedicalItems.Splint; }
        }

        public override MedicineKinds MedicineKind {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit {
            get { return 1963f; }
        }
    }
}