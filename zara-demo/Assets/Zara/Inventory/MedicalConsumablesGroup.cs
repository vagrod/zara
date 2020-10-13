using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Inventory
{
    public class MedicalConsumablesGroup
    {
        // Groups definition
        public static readonly MedicalConsumablesGroup EpinephrineGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.EpinephrineSyringe) {Name = "Epinephrine" };
        public static readonly MedicalConsumablesGroup AntiVenomGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.AntiVenomSyringe) { Name = "AntiVenom" };
        public static readonly MedicalConsumablesGroup AtropineGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.AtropineSyringe, "MyraFruit") { Name = "Atropine" };
        public static readonly MedicalConsumablesGroup MorphineGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.MorphineSyringe) { Name = "Morphine" };
        public static readonly MedicalConsumablesGroup DoripenemGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.DoripenemSyringe) { Name = "Doripenem" };

        public static readonly MedicalConsumablesGroup AntibioticGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.Antibiotic) { Name = "Antibiotic" };
        public static readonly MedicalConsumablesGroup AspirinGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.Aspirin) { Name = "Aspirin" };
        public static readonly MedicalConsumablesGroup AcetaminophenGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.Acetaminophen) { Name = "Acetaminophen" };
        public static readonly MedicalConsumablesGroup LoperamideGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.Loperamide) { Name = "Loperamide" };
        public static readonly MedicalConsumablesGroup OseltamivirGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.Oseltamivir) { Name = "Oseltamivir" };
        public static readonly MedicalConsumablesGroup SedativeGroup = new MedicalConsumablesGroup(InventoryController.MedicalItems.Sedative) { Name = "Sedative" };

        public static MedicalConsumablesGroup GetRelevantMedicalGroup(InventoryConsumableItemBase item)
        {
            if(EpinephrineGroup.IsApplicableToGroup(item))
                return EpinephrineGroup;

            if (AntiVenomGroup.IsApplicableToGroup(item))
                return AntiVenomGroup;

            if (AtropineGroup.IsApplicableToGroup(item))
                return AtropineGroup;

            if (MorphineGroup.IsApplicableToGroup(item))
                return MorphineGroup;

            if (AntibioticGroup.IsApplicableToGroup(item))
                return AntibioticGroup;

            if (AspirinGroup.IsApplicableToGroup(item))
                return AspirinGroup;

            if (AcetaminophenGroup.IsApplicableToGroup(item))
                return AcetaminophenGroup;

            if (LoperamideGroup.IsApplicableToGroup(item))
                return LoperamideGroup;

            if (OseltamivirGroup.IsApplicableToGroup(item))
                return OseltamivirGroup;

            if (SedativeGroup.IsApplicableToGroup(item))
                return SedativeGroup;

            if (DoripenemGroup.IsApplicableToGroup(item))
                return DoripenemGroup;

            return null;
        }

        private readonly string[] _items;

        public int MergedGroupsCount { get; private set; }

        public string[] Items
        {
            get { return _items; }
        }

        public bool IsApplicableToGroup(IInventoryItem item)
        {
            return Items.Any(x => x.ToLower() == item.Name.ToLower());
        }

        public string Name { get; set; }

        public MedicalConsumablesGroup(params string[] items)
        {
            _items = items;

            MergedGroupsCount = 1;
        }

        public static MedicalConsumablesGroup operator + (MedicalConsumablesGroup g1, MedicalConsumablesGroup g2)
        {
            var result = g1.Items.ToList();

            result.AddRange(g2.Items);

            var groupsCount = g1.MergedGroupsCount + g2.MergedGroupsCount + 1;

            return new MedicalConsumablesGroup(result.ToArray()){ MergedGroupsCount = groupsCount};
        }

    }
}
