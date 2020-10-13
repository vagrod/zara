using System;
using System.Collections.Generic;
using System.Linq;
using ZaraEngine.Inventory.Combinatory.Fluent;
using ZaraEngine.Inventory.Descriptions.Medical;

namespace ZaraEngine.Inventory
{
    public class InventoryItemsCombinatoryFactory
    {

        private InventoryItemsCombinatoryFactory()
        {
            _variants = new List<ItemsCombination>(new[]
            {
                /* Medical Combinations */
                CombinatoryItemBuilder      <SyringeOfAntiVenom>       .Is<EmptySyringe>().And<AntiVenomSolution>(),
                CombinatoryItemBuilder      <SyringeOfAtropine>        .Is<EmptySyringe>().And<AtropineSolution>(),
                CombinatoryItemBuilder      <SyringeOfEpinephrine>     .Is<EmptySyringe>().And<EpinephrineSolution>(),
                CombinatoryItemBuilder      <SyringeOfMorphine>        .Is<EmptySyringe>().And<MorphineSolution>(),
                CombinatoryItemBuilder      <SyringeOfDoripenem>       .Is<EmptySyringe>().And<DoripenemSolution>(),
                CombinatoryItemBuilder      <AntibioticEmbrocation>    .Is<DoripenemSolution>().And<Bandage>(),
                CombinatoryItemBuilder      <Headscarf>                .Is<NeedleAndThread>().And<Cloth>(3),
            });
        }

        private static InventoryItemsCombinatoryFactory _instance;

        public static InventoryItemsCombinatoryFactory Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new InventoryItemsCombinatoryFactory();

                return _instance;
            }
        }

        private readonly List<ItemsCombination> _variants;

        public ItemsCombination GetCombinationById(Guid id)
        {
            return _variants.FirstOrDefault(x => x.Id == id);
        }

        public List<ItemsCombination> GetMatchedCombinations(params IInventoryItem[] items)
        {
            var result = new List<ItemsCombination>();

            var itemsMatchedByLen = _variants.Where(x => x.ItemsNeeded.Count == items.Length);

            foreach (var itemInfo in itemsMatchedByLen)
            {
                var s1 = string.Join(";", itemInfo.ItemsNeeded.Select(x => x.ItemType.Name).OrderBy(x => x).ToArray());
                var s2 = string.Join(";", items.Select(x => x.GetType().Name).OrderBy(x => x).ToArray());

                if (s1 == s2)
                {
                    result.Add(itemInfo);
                }
            }

            return result;
        }

        private T ItemByType<T>(List<IInventoryItem> items)
            where T: InventoryItemBase
        {
            var type = typeof(T);
            var item = items.FirstOrDefault(x => x.GetType().Name == type.Name || x.GetType().IsSubclassOf(type));

            return item as T;
        }

    }

    public class ItemInSetInfo
    {
        public Type ItemType { get; private set; }
        public int Count { get; private set; }

        public ItemInSetInfo(Type itemType) : this(itemType, 1) { }

        public ItemInSetInfo(Type itemType, int count)
        {
            ItemType = itemType;
            Count = count;
        }
    }

    public class ItemsCombination
    {

        public enum SpecialActions
        {
            None = 0,
            TorchOff,
            DisinfectWater,
            WetTheMask,
            ChangeGasMaskFilter
        }

        public ItemsCombination()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public Type ResultType { get; set; }
        public List<ItemInSetInfo> ItemsNeeded { get; set; }

        public SpecialActions SpecialAction { get; set; }

        public Func<List<IInventoryItem>, InventoryCombinatoryResult> CheckForActionAvailability { get; set; }
        public Func<List<IInventoryItem>, IGameController, InventoryCombinatoryResult> GetIsValidCombination { get; set; }
    }

}
