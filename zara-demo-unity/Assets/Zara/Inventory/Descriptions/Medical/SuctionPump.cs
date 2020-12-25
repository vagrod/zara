namespace ZaraEngine.Inventory {
    public class SuctionPump : InventoryInfiniteMedicalItemBase {
        public override string Name {
            get { return InventoryController.MedicalItems.SuctionPump; }
        }

        public override MedicineKinds MedicineKind {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit {
            get { return 103.9f; }
        }
    }
}