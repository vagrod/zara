using System;
using System.Collections.Generic;
using System.Linq;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Inventory
{
    [Serializable]
    public class InventoryController : IAcceptsStateChange
    {

        public const float MaximumInventoryWeight = 55000f; // gramms

        public float RoughWeight { get; private set; }

        public float CurrentWeight
        {
            get
            {
                var weight = 0f;
                var list = Items.ToList(); // Using copy

                foreach (var item in list)
                {
                    // For every food item we count weight as [good food weight] + [spoiled food weight]
                    var food = item as FoodItemBase;
                    if (food != null)
                    {
                        var goodItem = GetByName(food.OriginalName);
                        var spoiledItem = GetByName(food.OriginalName + FoodItemBase.SpoiledPostfix);

                        if (goodItem != null)
                            weight += goodItem.WeightGrammsPerUnit * goodItem.Count;

                        if (spoiledItem != null)
                            weight += spoiledItem.WeightGrammsPerUnit * spoiledItem.Count;

                        continue;
                    }

                    // For every water vessel item we count weight as [water inside vessel weight] + [vessel weight itself]
                    var water = item as WaterVesselItemBase;
                    if (water != null)
                    {
                        weight += water.WaterDoseWeightInGramms * water.DosesCount + water.VesselWeightInGramms;

                        continue;
                    }

                    // For every clothes item we must check if we are wearing this item
                    var clothes = item as ClothesItemBase;
                    if (clothes != null)
                    {
                        if (_gc.Body.Clothes.Any(x => x.Name == item.Name))
                            continue;
                    }

                    // For everything else we count weight as [weight per unit] * [units count]
                    weight += item.WeightGrammsPerUnit * item.Count;
                }

                return weight;
            }
        }

        public enum InventoryItemType
        {
            Organic,
            Tool,
            Handheld,
            Medical,
            Clothes
        }

        public static class CommonObjects
        {

            public const string Water = "Water";
            public const string Medkit = "Medkit";
            public const string Campfire = "Campfire";

        }

        public static class CommonClothes
        {
            public const string WaterproofJacket = "WaterproofJacket";
            public const string WaterproofPants = "WaterproofPants";
            public const string RubberBoots = "RubberBoots";
            public const string LeafHat = "LeafHat";
        }

        public static class CommonTools
        {
            public const string Hand = "Hand";
            public const string Ash = "Ash";
            public const string Cloth = "Cloth";
            public const string DisinfectingPellets = "DisinfectingPellets";
            public const string Knife = "Knife";
            public const string NeedleAndThread = "NeedleAndThread";
            public const string Pin = "Pin";
            public const string Resin = "Resin";
            public const string Rope = "Rope";
            public const string StoneAxe = "StoneAxe";
            public const string Flask = "Flask";
        }

        public static class MedicalItems
        {
            public const string Acetaminophen = "Acetaminophen Pills";
            public const string Antibiotic = "Antibiotic Pills";
            public const string AntisepticSponge = "Antiseptic Sponge";
            public const string AntiVenomSolution = "Anti-venom Solution";
            public const string DoripenemSolution = "Doripenem Solution";
            public const string Aspirin = "Aspirin Pills";
            public const string AtropineSolution = "Atropine Solution";
            public const string Bandage = "Bandage";
            public const string EmptySyringe = "Empty Syringe";
            public const string Loperamide = "Loperamide Pills";
            public const string MorphineSolution = "Morphine Solution";
            public const string Plasma = "Plasma";
            public const string Sedative = "Sedative Pills";
            public const string SuctionPump = "Suction Pump";
            public const string BioactiveHydrogel = "Bioactive Hydrogel";
            public const string AntiVenomSyringe = "Syringe of Anti-venom";
            public const string AtropineSyringe = "Syringe of Atropine";
            public const string MorphineSyringe = "Syringe of Morphine";
            public const string EpinephrineSyringe = "Syringe of Epinephrine";
            public const string DoripenemSyringe = "Syringe of Doripenem";
            public const string Splint = "Splint";
            public const string AntibioticEmbrocation = "Antibiotic Embrocation";
            public const string EpinephrineSolution = "Epinephrine Solution";
            public const string Oseltamivir = "Oseltamivir Pills";
        }

        private readonly IGameController _gc;
        private readonly InventorySpecialActionsProcessor _combinationActionProcessor;

        private readonly Dictionary<string, bool> _itemsAvailabilityCache = new Dictionary<string, bool>();

        public List<IInventoryItem> Items { get; private set; }

        public InventoryController(IGameController gc)
        {
            _gc = gc;
            _combinationActionProcessor = new InventorySpecialActionsProcessor(gc);

            Items = new List<IInventoryItem>();
        }

        public InventoryCombinatoryResult CheckCombinationForResourcesAvailability(Guid combinationGuid)
        {
            var combination = InventoryItemsCombinatoryFactory.Instance.GetCombinationById(combinationGuid);

            return CheckCombinationForResourcesAvailability(combination);
        }

        private InventoryCombinatoryResult CheckCombinationForResourcesAvailability(ItemsCombination combination)
        {
            if (combination != null)
            {
                // Special check for special actions
                if (combination.SpecialAction != ItemsCombination.SpecialActions.None)
                {
                    var invItems = combination.ItemsNeeded.Select(item => Items.FirstOrDefault(x => x.GetType().Name == item.ItemType.Name)).ToList();

                    var result = combination.CheckForActionAvailability.Invoke(invItems);

                    if (result.Result != InventoryCombinatoryResult.CombinatoryResult.Allowed)
                        return result;
                }

                // Validate combination via validation function
                if (combination.GetIsValidCombination != null)
                {
                    var invItems = combination.ItemsNeeded.Select(item => Items.FirstOrDefault(x => x.GetType().Name == item.ItemType.Name)).ToList();
                    var validationResult = combination.GetIsValidCombination(invItems, _gc);

                    if (validationResult.Result != InventoryCombinatoryResult.CombinatoryResult.Allowed)
                        return validationResult;
                }

                // Let's check if we have resources for this
                foreach (var item in combination.ItemsNeeded)
                {
                    var invItem = Items.FirstOrDefault(x => x.GetType().Name == item.ItemType.Name);

                    if (invItem != null)
                    {
                        var wasteInfo = WasteItem(invItem, item.Count, checkOnly: true);
                        if (wasteInfo.Result == WasteCheckInfo.WasteResult.InsufficientResources)
                            return new InventoryCombinatoryResult
                            {
                                Result = InventoryCombinatoryResult.CombinatoryResult.InsufficientResources,
                                ResultedItem = null,
                                ResourcesWasteInfo = combination.ItemsNeeded
                            };
                    }
                    else
                    {
                        // We don't even have this item
                        return new InventoryCombinatoryResult
                        {
                            Result = InventoryCombinatoryResult.CombinatoryResult.InsufficientResources,
                            ResultedItem = null,
                            ResourcesWasteInfo = combination.ItemsNeeded
                        };
                    }
                }
            }
            else
            {
                return new InventoryCombinatoryResult { Result = InventoryCombinatoryResult.CombinatoryResult.CombinationDoesNotExist };
            }

            return new InventoryCombinatoryResult{Result = InventoryCombinatoryResult.CombinatoryResult.Allowed, ResourcesWasteInfo = combination.ItemsNeeded };
        }

        public string GetItemCompleteName(IInventoryItem item)
        {
            var water = item as WaterVesselItemBase;

            if (water != null)
                return water.Name + water.NamePostfix;

            return item.Name;
        }

        public InventoryCombinatoryResult TryCombine(Guid combinationGuid, bool checkOnly)
        {
            var combination = InventoryItemsCombinatoryFactory.Instance.GetCombinationById(combinationGuid);

            if (combination != null)
            {
                if (checkOnly)
                    return CheckCombinationForResourcesAvailability(combination);

                if (ProcessSpecialAction(combination))
                    return new InventoryCombinatoryResult { IsViaSpecialAction = true, Result = InventoryCombinatoryResult.CombinatoryResult.Allowed, ResultedItem = null, ResourcesWasteInfo = null };

                // Let's check if we have resources for this
                var resourcesCheckResult = CheckCombinationForResourcesAvailability(combination);

                if (resourcesCheckResult.Result != InventoryCombinatoryResult.CombinatoryResult.Allowed)
                    return resourcesCheckResult;

                // Let's actually waste these items
                foreach (var item in combination.ItemsNeeded)
                {
                    var invItem = Items.FirstOrDefault(x => x.GetType().Name == item.ItemType.Name);
                    WasteItem(invItem, item.Count, checkOnly: false);
                }

                var invItemResulted = Items.FirstOrDefault(x => x.GetType().Name == combination.ResultType.Name);
                IInventoryItem itemAffected = null;

                if (invItemResulted == null)
                {
                    var newItem = (IInventoryItem)Activator.CreateInstance(combination.ResultType);

                    itemAffected = newItem;

                    AddItem(newItem);
                }
                else
                {
                    itemAffected = invItemResulted;
                    invItemResulted.Count++;
                }

                return new InventoryCombinatoryResult { Result = InventoryCombinatoryResult.CombinatoryResult.Allowed, ResultedItem = itemAffected, ResourcesWasteInfo = combination.ItemsNeeded };
            }

            return new InventoryCombinatoryResult {Result = InventoryCombinatoryResult.CombinatoryResult.CombinationDoesNotExist, ResultedItem = null, ResourcesWasteInfo = null};
        }

        private bool ProcessSpecialAction(ItemsCombination combination)
        {
            return _combinationActionProcessor.ProcessAction(combination).IsAllowed;
        }

        internal WasteCheckInfo WasteItem(IInventoryItem item, int countToWaste, bool checkOnly)
        {
            var food = item as FoodItemBase;

            if (item is WaterVesselItemBase)
            {
                var vessel = (WaterVesselItemBase)item;

                if (vessel.DosesLeft < countToWaste)
                    return new WasteCheckInfo {Item = item, Result = WasteCheckInfo.WasteResult.InsufficientResources}; // Insufficient resources

                if (!checkOnly)
                {
                    for (int i = 0; i < countToWaste; i++)
                        vessel.TakeAwayOneDose();
                }

                return new WasteCheckInfo { Item = item, Result = WasteCheckInfo.WasteResult.Allowed }; ;
            }

            if (item is IInventoryInfiniteItem)
                return new WasteCheckInfo { Item = item, Result = WasteCheckInfo.WasteResult.InfiniteResource }; ; // Infinite resource

            if (item.Count < countToWaste)
                return new WasteCheckInfo { Item = item, Result = WasteCheckInfo.WasteResult.InsufficientResources }; ; // Insufficient resources

            if (!checkOnly)
            {
                if (item.Count == countToWaste)
                {
                    if (food != null)
                    {
                        var original = Items.FirstOrDefault(x => x.Name == food.OriginalName) as FoodItemBase;

                        if (food.IsSpoiled)
                        {
                            if (original != null)
                                original.RemoveAllSpoiled(_gc.WorldTime.Value);
                        }
                        else
                        {
                            if (original != null)
                                original.RemoveAllNormal(_gc.WorldTime.Value);
                        }

                        original.Count -= countToWaste;
                    }
                    else
                    {
                        RemoveItem(item.Name, null);
                    }

                    return new WasteCheckInfo { Item = item, Result = WasteCheckInfo.WasteResult.UsedAll };
                }
                else
                {
                    if (food != null)
                    {
                        var original = Items.FirstOrDefault(x => x.Name == food.OriginalName) as FoodItemBase;

                        if (original != null)
                        {
                            for (int j = 0; j < countToWaste; j++)
                            {
                                if (food.IsSpoiled)
                                    original.TakeOneFromSpoiledGroup(_gc.WorldTime.Value);
                                else
                                    original.TakeOneFromNormalGroup(_gc.WorldTime.Value);
                            }

                            original.Count -= countToWaste;
                        }
                    }

                    item.Count -= countToWaste;
                }
            }

            return new WasteCheckInfo { Item = item, Result = WasteCheckInfo.WasteResult.Allowed };
        }

        public T GetByType<T>()
            where T : IInventoryItem
        {
            return (T)Items.FirstOrDefault(x => x is T);
        }

        public IInventoryItem GetByName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            if (name.Contains("$"))
            {
                // Special request
                return ProcessSpecialGet(name);
            }

            if (!HasItem(name))
                return null;

            var item = Items.First(x => x.Name == name);

            var food = item as FoodItemBase;
            if (food != null)
            {
                var result = (FoodItemBase)Activator.CreateInstance(item.GetType());

                result.IsSpoiled = false;
                result.Count = food.GetCountNormal(_gc.WorldTime.Value);

                return result;
            }

            return item;
        }

        private IInventoryItem ProcessSpecialGet(string nameInfo)
        {
            var info = nameInfo.Split('$');
            var name = info[0];
            var state = "$" + info[1];

            if (!HasItem(name))
                return null;

            var item = Items.FirstOrDefault(x => x.Name == name || x.Name == nameInfo);

            if (item == null)
                return null;

            var food = item as FoodItemBase;
            if (food != null)
            {
                if (state == FoodItemBase.SpoiledPostfix)
                {
                    var result = (FoodItemBase)Activator.CreateInstance(item.GetType());

                    result.Count = food.GetCountSpoiled(_gc.WorldTime.Value);
                    result.IsSpoiled = true;

                    return result;
                }
            }

            return item;
        }

        public ItemUseResult TryUse(IInventoryItem item, bool checkOnly)
        {
            var isSpoiledFoodUsedAll = false;
            var isFreshFoodUsedAll = false;
            var originalItem = item;

            var food = item as FoodItemBase;
            if (food != null)
            {
                var itemName = item.Name;

                if (item.Name.Contains('$'))
                    itemName = item.Name.Split('$')[0];

                var inventoryObject = (FoodItemBase)Items.FirstOrDefault(x => x.Name == itemName);

                if (inventoryObject == null)
                    return new ItemUseResult
                    {
                        Item = null,
                        Result = ItemUseResult.UsageResult.InsufficientResources
                    };

                if (food.IsSpoiled)
                    isSpoiledFoodUsedAll = inventoryObject.TakeOneFromSpoiledGroup(_gc.WorldTime.Value) == 0;
                else
                    isFreshFoodUsedAll = inventoryObject.TakeOneFromNormalGroup(_gc.WorldTime.Value) == 0;

                item = inventoryObject;
            }

            var vessel = item as WaterVesselItemBase;
            if (vessel != null)
            {
                if (vessel.DosesLeft < 1)
                    return new ItemUseResult { Item = vessel, Result = ItemUseResult.UsageResult.InsufficientResources};

                if (!checkOnly)
                {
                    _gc.Health.OnConsumeItem(vessel);

                    vessel.TakeAwayOneDose();

                    Events.NotifyAll(l => l.Drink(_gc));

                    RefreshRoughWeight();

                    if (vessel.DosesLeft == 0)
                        return new ItemUseResult { Item = vessel, Result = ItemUseResult.UsageResult.UsedAll };
                }

                if (checkOnly && vessel.DosesLeft - 1 == 0)
                    return new ItemUseResult { Item = vessel, Result = ItemUseResult.UsageResult.UsedAll };

                return new ItemUseResult { Item = vessel, Result = ItemUseResult.UsageResult.UsedSingle };
            }

            var medItem = item as InventoryMedicalItemBase;
            if (item.Type.Contains(InventoryItemType.Organic) || (medItem != null && medItem.MedicineKind == InventoryMedicalItemBase.MedicineKinds.Consumable))
            {
                if (!checkOnly)
                {
                    _gc.Health.OnConsumeItem(originalItem as InventoryConsumableItemBase);
                }

                if (item.Count == 0)
                    return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.InsufficientResources };

                if (item.Count == 1)
                {
                    if (!(item is IInventoryInfiniteItem))
                    {
                        var foodCount = -1;

                        if(food != null){
                            foodCount = food.GetCountNormal(_gc.WorldTime.Value) + food.GetCountSpoiled(_gc.WorldTime.Value);
                        }

                        if (!checkOnly)
                        {
                            if (food == null)
                            {
                                RemoveItem(item.Name, null);
                            } else {
                                if(foodCount == 0)
                                    Items.Remove(item);
                            }
                        }

                        if(food == null)
                            return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.UsedAll };
                        else {
                            if(foodCount == 0)
                                return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.UsedAll };
                            else 
                                return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.UsedSingle };
                        }
                    }
                    else
                    {
                        return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.UsedSingle };
                    }
                }
                else
                {
                    if (!checkOnly)
                    {
                        if (!(item is IInventoryInfiniteItem))
                        {
                            item.Count--;

                            RefreshRoughWeight();
                        }
                    }

                    // Special case: return all/single for spoiled and fresh food separately
                    if(isFreshFoodUsedAll || isSpoiledFoodUsedAll)
                        return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.UsedAll };
                    else
                        return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.UsedSingle };
                }
            }

            if (medItem != null && medItem.MedicineKind == InventoryMedicalItemBase.MedicineKinds.Appliance)
            {
                // Actual applying is handled in a medcenter. Here we know nothing about the body part.
                if (item.Count == 0)
                    return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.InsufficientResources };

                if (item.Count == 1)
                {
                    if (!(item is IInventoryInfiniteItem))
                    {
                        if (!checkOnly)
                            RemoveItem(item.Name, null);

                        return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.UsedAll };
                    }
                    else
                    {
                        return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.UsedSingle };
                    }
                }
                else
                {
                    if (!checkOnly)
                    {
                        if (!(item is IInventoryInfiniteItem))
                        {
                            item.Count--;

                            RefreshRoughWeight();
                        }
                    }

                    return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.UsedSingle };
                }
            }

            return new ItemUseResult { Item = item, Result = ItemUseResult.UsageResult.Unapplicable };
        }

        public void AddItem(IInventoryItem newItem)
        {
            var cnt = newItem.Count;

            if (cnt == 0)
                cnt = 1;

            if (HasItem(newItem.Name))
            {
                var inventoryItem = Items.First(x => x.Name == newItem.Name);    

                var food = inventoryItem as FoodItemBase;
                if (food != null)
                {
                    food.AddGatheringInfo(_gc.WorldTime.Value, cnt);
                }

                inventoryItem.Count += cnt;

                return;
            }

            newItem.Count = cnt;

            var newFood = newItem as FoodItemBase;
            if (newFood != null)
            {
                newFood.AddGatheringInfo(_gc.WorldTime.Value, cnt);
            }

            Items.Add(newItem);

            _itemsAvailabilityCache.Clear();

            RefreshRoughWeight();
        }

        public void RemoveItem(string itemName, int? count)
        {
            var item = GetByName(itemName);

            if (item == null)
                return;

            if (count.HasValue)
            {
                WasteItem(item, count.Value, checkOnly: false);

                RefreshRoughWeight();

                return;
            }

            var food = item as FoodItemBase;

            if (food != null)
            {
                WasteItem(food, food.Count, checkOnly: false);

                return;
            }

            Items.Remove(item);
            _itemsAvailabilityCache.Clear();

            RefreshRoughWeight();
        }

        public int CountOf(string name)
        {
            if (!HasItem(name))
                return 0;

            var item = GetByName(name);

            if (item == null)
                return 0;

            return item.Count;
        }

        public bool HasItem(string name)
        {
            if (name == CommonTools.Hand)
                return true;

            if (name.Contains("$"))
            {
                var info = name.Split('$');
                var realName = info[0];

                return HasItem(realName);
            }

            if (_itemsAvailabilityCache.Count == 0)
                RebuildCache();

            if (!_itemsAvailabilityCache.ContainsKey(name))
                _itemsAvailabilityCache.Add(name, false);

            return _itemsAvailabilityCache[name];
        }

        private void RebuildCache()
        {
            _itemsAvailabilityCache.Clear();

            Items.ForEach(item => _itemsAvailabilityCache.Add(item.Name, true));
        }

        public void RefreshRoughWeight()
        {
            RoughWeight = CurrentWeight;
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var generic = Items.Where(x => !(x is FoodItemBase) && !(x is WaterVesselItemBase)).ToList();
            var food = Items.Where(x => x is FoodItemBase).ToList();
            var water = Items.Where(x => x is WaterVesselItemBase).ToList();

            var state = new InventoryControllerStateSnippet
            {
                RoughWeight = this.RoughWeight,

                GenericInventoryItems = generic.ConvertAll(x => (InventoryItemSnippet)(x as InventoryItemBase).GetState()),
                FoodInventoryItems = food.ConvertAll(x => (InventoryFoodItemSnippet)(x as FoodItemBase).GetState()),
                WaterInventoryItems = water.ConvertAll(x => (InventoryWaterVesselItemSnippet)(x as WaterVesselItemBase).GetState())
            };

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (InventoryControllerStateSnippet)savedState;

            RoughWeight = state.RoughWeight;

            Items.Clear();

            var mapping = new Dictionary<Guid, Guid>(); //old id, new id

            foreach(var itemData in state.GenericInventoryItems)
            {
                var newItem = (InventoryItemBase)Activator.CreateInstance(itemData.ItemType);

                newItem.RestoreState(itemData);

                mapping.Add(itemData.Id, newItem.Id);

                Items.Add(newItem);
            }

            foreach (var itemData in state.FoodInventoryItems)
            {
                var newItem = (InventoryItemBase)Activator.CreateInstance(itemData.ItemType);

                newItem.RestoreState(itemData);

                mapping.Add(itemData.Id, newItem.Id);

                Items.Add(newItem);
            }

            foreach (var itemData in state.WaterInventoryItems)
            {
                var newItem = (InventoryItemBase)Activator.CreateInstance(itemData.ItemType);

                newItem.RestoreState(itemData);

                mapping.Add(itemData.Id, newItem.Id);

                Items.Add(newItem);
            }

            state.SetItemsMapping(mapping);

            RebuildCache();
        }

        #endregion
    }

    public class InventoryCombinatoryResult
    {

        public enum CombinatoryResult
        {
            Allowed,
            InsufficientResources,
            CombinationDoesNotExist
        }

        public bool IsViaSpecialAction { get; set; }
        public IInventoryItem ResultedItem { get; set; }
        public List<ItemInSetInfo> ResourcesWasteInfo { get; set; }
        public CombinatoryResult Result { get; set; }
    }

    public class ItemUseResult
    {
        public enum UsageResult
        {
            UsedSingle,
            UsedAll,
            InsufficientResources,
            Unapplicable
        }

        public IInventoryItem Item { get; set; }

        public UsageResult Result { get; set; }

    }

    public class WasteCheckInfo
    {

        public enum WasteResult
        {
            Allowed,
            InsufficientResources,
            InfiniteResource,
            UsedAll
        }

        public IInventoryItem Item { get; set; }
        public WasteResult Result { get; set; }
    }

}
