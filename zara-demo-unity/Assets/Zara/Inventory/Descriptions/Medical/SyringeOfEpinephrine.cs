namespace ZaraEngine.Inventory {
    public class SyringeOfEpinephrine : InventoryMedicalItemBase {

        public override string Name {
            get { return InventoryController.MedicalItems.EpinephrineSyringe; }
        }

        public override MedicineKinds MedicineKind {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit {
            get { return 11.6f; }
        }

    }
}