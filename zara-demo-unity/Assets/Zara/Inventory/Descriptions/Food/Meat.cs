namespace ZaraEngine.Inventory
{
    public class Meat : FoodItemBase
    {

        public Meat()
        {
            SpoiledChanceOfFoodPoisoning = 72; // Chance of food poisoning when meat is spoiled
            GeneralChanceOfFoodPoisoning = 58; // Chance of food poisoning for this food in general
        }

        public override string OriginalName
        {
            get { return "Meat"; }
        }

        public override float FoodValue
        {
            get { return 40 + Helpers.RollDice(1, 10); }
        }

        public override float WaterValue
        {
            get { return 17 + Helpers.RollDice(1, 10); }
        }

        public override int MinutesUntilSpoiled
        {
            get { return 60 * 5; }
        }

        public override float WeightGrammsPerUnit
        {
            get
            {
                const int RawItemWeight = 632;

                if (IsSpoiled)
                    return RawItemWeight - 80f;

                return RawItemWeight;
            }
        }
    }
}