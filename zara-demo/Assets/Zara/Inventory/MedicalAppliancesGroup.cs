using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public class MedicalAppliancesGroup
    {

        public static MedicalAppliancesGroup AntisepticGroup = new MedicalAppliancesGroup(InventoryController.MedicalItems.AntisepticSponge, InventoryController.CommonTools.Ash);

        public List<string> Items { get; private set; }

        public MedicalAppliancesGroup(params string[] tools)
        {
            Items = tools.ToList();
        }

        public static MedicalAppliancesGroup GetRelevantMedicalGroup(InventoryMedicalItemBase item)
        {
            return GetRelevantMedicalGroup(item.Name);
        }

        public static MedicalAppliancesGroup GetRelevantMedicalGroup(string itemName)
        {
            if (AntisepticGroup.IsApplicableToGroup(itemName))
                return AntisepticGroup;

            return null;
        }

        public bool IsApplicableToGroup(IInventoryItem item)
        {
            return IsApplicableToGroup(item.Name);
        }

        public bool IsApplicableToGroup(string itemName)
        {
            return Items.Any(x => x.ToLower() == itemName.ToLower());
        }

    }
}
