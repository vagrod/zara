using System;
using System.Collections.Generic;
using System.Linq;

namespace ZaraEngine.Inventory
{
    public class ClothesGroups
    {

        public List<ClothesGroup> Groups { get; private set; }

        private ClothesGroups()
        {
            Groups = new List<ClothesGroup>(new[]
            {
                new ClothesGroup
                {
                    Name = "WaterResistantSuit",
                    ColdResistanceBonus = 0,
                    WaterResistanceBonus = 25,
                    Members =
                    {
                        InventoryController.CommonClothes.WaterproofJacket,
                        InventoryController.CommonClothes.WaterproofPants,
                        InventoryController.CommonClothes.RubberBoots,
                        InventoryController.CommonClothes.LeafHat
                    }
                }, 
            });

            Groups.ForEach(x => x.Initialize());
        }

        private static ClothesGroups _instance;

        public static ClothesGroups Instance => _instance ?? (_instance = new ClothesGroups());

        public ClothesGroup GetCompleteClothesGroup(IGameController gc)
        {
            if (gc.Body.Clothes.Count == 0)
                return null;

            return Groups.FirstOrDefault(x => x.Members.All(member => gc.Body.Clothes.Any(c => member == c.Name)));
        }

        public List<ClothesGroup> GetPossibleClothesGroups(IGameController gc)
        {
            if (gc.Body.Clothes.Count == 0)
                return new List<ClothesGroup>();

            return Groups.Where(x => x.Members.Any(member => gc.Body.Clothes.Any(c => member == c.Name))).ToList();
        }

        public class ClothesGroup
        {

            public ClothesGroup()
            {
                Members = new List<string>();
            }

            public List<string> Members { get; private set; }

            public string Name { get; set; }

            public int WaterResistanceBonus{ get; set; }

            public int ColdResistanceBonus { get; set; }

            public int TotalWaterProtection { get; private set; }

            public int TotalColdProtection { get; private set; }

            public void Initialize()
            {
                var waterProtectionValue = WaterResistanceBonus;
                var coldProtectionValue = ColdResistanceBonus;

                Members.ForEach(itemName =>
                {
                    var item = (IInventoryItem)Activator.CreateInstance(Type.GetType($"ZaraEngine.Inventory.{itemName}"));

                    var clothes = item as ClothesItemBase;
                    if (clothes != null)
                    {
                        waterProtectionValue += clothes.WaterResistance;
                        coldProtectionValue += clothes.ColdResistance;
                    }
                });

                TotalColdProtection = coldProtectionValue;
                TotalWaterProtection = waterProtectionValue;
            }

        }

    }
}
