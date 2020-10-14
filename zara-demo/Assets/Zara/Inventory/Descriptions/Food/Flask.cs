namespace ZaraEngine.Inventory
{
    public class Flask : WaterVesselItemBase
    {

        public override string Name
        {
            get { return InventoryController.CommonTools.Flask; }
        }

        public override int WaterValuePerDose
        {
            get { return 21; }
        }

        public override float VesselWeightInGramms
        {
            get { return 250f; }
        }

        public override float WaterDoseWeightInGramms
        {
            get { return 304f; }
        }

        public override int DosesCount
        {
            get { return 3; }
        }
    }
}