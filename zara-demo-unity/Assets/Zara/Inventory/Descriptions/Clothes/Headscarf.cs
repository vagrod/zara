namespace ZaraEngine.Inventory
{
    public class Headscarf : ClothesItemBase
    {
        public override string Name
        {
            get { return "Headscarf"; }
        }

        public override ClothesTypes ClothesType
        {
            get { return ClothesTypes.Hat; }
        }

        public override int WaterResistance
        {
            get { return 4; }
        }

        public override int ColdResistance
        {
            get { return 6; }
        }

        public override float WeightGrammsPerUnit
        {
            get { return 311f; }
        }
    }
}