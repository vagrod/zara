namespace ZaraEngine.Inventory {
    public class SyringeOfAntiVenom : InventoryMedicalItemBase {
        public override string Name {
            get { return InventoryController.MedicalItems.AntiVenomSyringe; }
        }

        public override MedicineKinds MedicineKind {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit {
            get { return 14.3f; }
        }
    }
}