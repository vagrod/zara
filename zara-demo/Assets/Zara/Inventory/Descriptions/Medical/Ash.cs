namespace ZaraEngine.Inventory {
    public class Ash : InventoryMedicalToolItemBase {
        public override string Name {
            get { return InventoryController.CommonTools.Ash; }
        }

        public override MedicineKinds MedicineKind {
            get { return MedicineKinds.Appliance; }
        }

        public override float WeightGrammsPerUnit {
            get { return 94; }
        }
    }
}
