using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ZaraEngine;
using ZaraEngine.Player;
using ZaraEngine.Inventory;
using ZaraEngine.HealthEngine;
using System.Text;
using ZaraEngine.StateManaging;

public class GameController : MonoBehaviour, IGameController
{

    private DateTime _dateTime;

    private PlayerStatus _player;

    private BodyStatusController _body;

    private WeatherDescription _weather;

    private HealthController _health;

    private InventoryController _inventory;

    private TimesOfDay _timeOfDay;

    #region Demo app fields

    private float _infoUpdateCounter;
    private float _dateTimeCounter;

    // our clothes references
    private ZaraEngine.Inventory.WaterproofJacket _jacket;
    private ZaraEngine.Inventory.WaterproofPants _pants;
    private ZaraEngine.Inventory.RubberBoots _boots;
    private ZaraEngine.Inventory.LeafHat _hat;

    #endregion 

    #region UI elements for data display

    public Text BodyTempText;
    public Text BloodPressureText;
    public Text FatigueLevelText;
    public Text HeartRateText;
    public Text FoodLevelText;
    public Text WaterLevelText;
    public Text BloodLevelText;
    public Text StaminaText;
    public Text OxygenLevelText;

    public Text AppliancesText;

    public Text DiseasesInfoText;
    public Text InjuriesInfoText;
    public Text CanEatText;
    public Text CanSleepText;
    public Text CanRunText;
    public Text HasLegFractureText;
    public Text WetnessLevelText;
    public Text WarmthLevelText;

    public Text LastSleepTimeText;
    public Text LastHealthCheckTimeText;

    public Button RunButton;
    public Button WalkButton;

    public Dropdown FirstInventoryItemsList;
    public Dropdown SecondInventoryItemsList;
    public Text WeightText;
    public Text AgentsSummaryText;
    public Text CurrentTimeText;

    public Text SleepingText;
    public Text SpeedText;
    public Text LimpText;

    #endregion 

    // Start is called before the first frame update
    void Start()
    {
        /* Zara Initialization code start =======>> */

        ZaraEngine.Helpers.InitializeRandomizer(UnityEngine.Random.Range);

        _dateTime = DateTime.Now;
        _timeOfDay = TimesOfDay.Evening;
        _health = new HealthController(this);
        _body = new BodyStatusController(this);
        _weather = new WeatherDescription();
        _player = new PlayerStatus();
        _inventory = new InventoryController(this);

        _body.Initialize();
        _health.Initialize();

        /* <<======= Zara Initialization code end */

        #region Demo app init

        // Let's add some items to the inventory to play with in this demo

        var flaskWithWater = new ZaraEngine.Inventory.Flask();

        flaskWithWater.FillUp(WorldTime.Value);
        //flaskWithWater.Disinfect(WorldTime.Value);

        _jacket = new ZaraEngine.Inventory.WaterproofJacket();
        _pants = new ZaraEngine.Inventory.WaterproofPants();
        _boots = new ZaraEngine.Inventory.RubberBoots();
        _hat = new ZaraEngine.Inventory.LeafHat();

        _inventory.AddItem(flaskWithWater);

        _inventory.AddItem(_jacket);
        _inventory.AddItem(_pants);
        _inventory.AddItem(_boots);
        _inventory.AddItem(_hat);

        var meat = new ZaraEngine.Inventory.Meat { Count = 1 };

        // We just gathered two of spoiled Meat. If will spoil in MinutesUntilSpoiled game minutes
        meat.AddGatheringInfo(WorldTime.Value.AddHours(-5), 2);
        
        // We just gathered one item on fresh food
        meat.AddGatheringInfo(WorldTime.Value, 1);

        _inventory.AddItem(new ZaraEngine.Inventory.Cloth { Count = 20 });
        _inventory.AddItem(meat); 
        _inventory.AddItem(new ZaraEngine.Inventory.AntisepticSponge { Count = 5 });
        _inventory.AddItem(new ZaraEngine.Inventory.Bandage { Count = 5 });
        _inventory.AddItem(new ZaraEngine.Inventory.Acetaminophen { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.Antibiotic { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.Aspirin { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.EmptySyringe { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.Loperamide { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.Oseltamivir { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.Sedative { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.AtropineSolution { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.EpinephrineSolution { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.AntiVenomSolution { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.DoripenemSolution { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.MorphineSolution { Count = 10 });
        _inventory.AddItem(new ZaraEngine.Inventory.DisinfectingPellets { Count = 5 });

        RefreshConsumablesUICombo();
        RefreshToolsUICombo();

        // Defaults
        _weather.SetTemperature(27f);
        _weather.SetWindSpeed(2f);
        _weather.SetRainIntensity(0f);

        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        /* These two calls are required by Zara */
        
        _body.Check(Time.deltaTime);
        _health.Check(Time.deltaTime);

        #region Demo UI update

        /* Updating UI info */

        _infoUpdateCounter += Time.deltaTime;
        _dateTimeCounter += Time.deltaTime;

        if(_dateTimeCounter >= 0.05f){
            _dateTime = _dateTime.AddSeconds(_dateTimeCounter * 10f); // in-game time is 10x the real one
            _dateTimeCounter = 0f;
        }

        if (_infoUpdateCounter >= 1f)
        {
            SpeedText.text = $"Speed: Running = {_player.RunSpeed:0.0}, Walking = {_player.WalkSpeed:0.0}, Crouching = {_player.CrouchSpeed:0.0}";
            LimpText.text = $"Is Limping? {(_player.IsLimping ? "yes" : "no")}";
            WeightText.text = $"{($"{ _inventory.CurrentWeight:# ### ##0}".Trim())} grams";
            CurrentTimeText.text = $"World time is {WorldTime.Value:MMMM dd, HH:mm:ss}";

            BodyTempText.text = $"Body Temp.: {_health.Status.BodyTemperature:#0.0} deg C";
            BloodPressureText.text = $"Blood Pressure: {_health.Status.BloodPressureTop:000}/{_health.Status.BloodPressureBottom:#00}";
            FatigueLevelText.text = $"Fatigue: {_health.Status.FatiguePercentage:#0.0}%";
            HeartRateText.text = $"Heart Rate: {_health.Status.HeartRate:00}bpm";
            FoodLevelText.text = $"Foood Level: {_health.Status.FoodPercentage:#0.0}%";
            WaterLevelText.text = $"Water Level: {_health.Status.WaterPercentage:#0.0}%";
            WetnessLevelText.text = $"Is Wet? {(_body.IsWet ? "yes" : "no")} (Wetness Level is {_body.WetnessLevel:#0.0}%)";
            WarmthLevelText.text = $"Warmth Score is {_body.GetWarmthLevel():0.0} [-5..+5 is a comfort warm feel]";
            BloodLevelText.text = $"Blood Level: {_health.Status.BloodPercentage:#0.0}% (Blood Loss? {(_health.Status.IsBloodLoss ? "yes" : "no")})";
            StaminaText.text = $"Stamina Level: {_health.Status.StaminaPercentage:#0.0}%";
            OxygenLevelText.text = $"Oxygen Level: {_health.Status.OxygenPercentage:#0.0}%";

            LastSleepTimeText.text = $"Last Time Slept: {_health.Status.LastSleepTime:MMMM dd, HH:mm}";
            LastHealthCheckTimeText.text = $"Last Health Update: {_health.Status.CheckTime:MMMM dd, HH:mm:ss}";

            CanEatText.text = $"Can Eat? {(_health.Status.IsFoodDisgust ? "no" : "yes")}";
            CanSleepText.text = $"Has Sleep Disorder? {(_health.Status.IsSleepDisorder ? "yes" : "no")}";
            CanRunText.text = $"Can Run? {(_health.Status.CannotRun ? "no" : "yes")}";
            HasLegFractureText.text = $"Has Leg Fracture? {(_health.Status.IsLegFracture ? "yes" : "no")}";
            SleepingText.text = $"Is Sleeping? {(_body.IsSleeping ? "yes" : "no")}";

            var sb = new StringBuilder();

            sb.AppendLine("Clothes:");

            var cg = ClothesGroups.Instance.GetCompleteClothesGroup();
            
            sb.AppendLine($"  Has Complete Set of Clothes? {(cg == null ? "no" : ("yes -- " + cg.Name + $"\n  Gives extra Water ({cg.WaterResistanceBonus}%) and Cold Resistance ({cg.ColdResistanceBonus}%) when complete set"))}");

            foreach(var item in _body.Clothes){
                sb.AppendLine($"\t• {item.Name} ({item.ClothesType})");
                sb.AppendLine($"\t  Water Resistance: {item.WaterResistance}%");
                sb.AppendLine($"\t  Cold Resistance: {item.ColdResistance}%");
            }

            sb.AppendLine();
            sb.AppendLine("Appliances:");

            if(_body.Appliances.Count == 0){
                sb.AppendLine("\t None");
            } else {
                foreach(var item in _body.Appliances){
                    sb.AppendLine($"\t• {item.Item.Name} on {item.BodyPart}");
                }
            }

            AppliancesText.text = sb.ToString();

            sb.Clear();

            sb.AppendLine($"Has Active Disease (with an Active Stage): {(_health.Status.IsActiveDisease ? "yes" : "no")}");
            sb.AppendLine($"Diseases count (including scheduled): {_health.Status.ActiveDiseases.Count}");

            foreach(var d in _health.Status.ActiveDiseases)
            {
                sb.AppendLine($"\t• {d.Disease.Name} ({d.Disease.Stages.Count} stages total)");

                var st = d.GetActiveStage(WorldTime.Value);

                if (st == null)
                {
                    // Scheduled disease
                    sb.AppendLine($"\t  Scheduled for {(d.DiseaseStartTime.ToString("HH:mm"))}");
                } else
                {
                    // Active disease
                    sb.AppendLine($"\t  {(d.TreatedStage == null ? "Active" : "Getting Better")}. Current stage is {st.Level}, stage will end at {(st.WillEndAt.HasValue ? $"{st.WillEndAt.Value:HH:mm}" : "n/a")}");
                }

                if(d.LinkedInjury != null){
                    sb.AppendLine($"\t  This disease was caused by {d.LinkedInjury.Injury.Name} injury");    
                }
                
                sb.AppendLine($"\t  Is treatment needed? {(d.IsSelfHealing ? "no" : "yes")}");
                sb.AppendLine();
            }

            DiseasesInfoText.text = sb.ToString();

            sb.Clear();

            sb.AppendLine($"Has Active Injury: {(_health.Status.IsActiveInjury ? "yes" : "no")}");

            foreach(var inj in _health.Status.ActiveInjuries){
                sb.AppendLine($"\t• {inj.Injury.Name} ({inj.BodyPart}) -- {inj.Injury.Stages.Count} stages total");

                var st = inj.GetActiveStage(WorldTime.Value);

                if(st == null){
                    // Scheduled injury
                    sb.AppendLine($"\t  Scheduled for {inj.InjuryTriggerTime:HH:mm}");
                } else {
                    // Active Injury
                    if(st.SelfHealAt.HasValue){
                        sb.AppendLine($"\t  Active. Current stage is {st.Level}, will self-heal at {st.SelfHealAt.Value:HH:mm}");
                    } else
                    {
                        sb.AppendLine($"\t  {(inj.TreatedStage == null ? "Active" : "Getting Better")}. Current stage is {st.Level}, stage will end at {(st.WillEndAt.HasValue ? $"{st.WillEndAt.Value:HH:mm}": "n/a")}");
                    }
                }

                if(st != null)
                    sb.AppendLine($"\t  Is treatment needed? {(st.SelfHealAt.HasValue ? "no" : "yes")}");

                sb.AppendLine();
            }

            InjuriesInfoText.text = sb.ToString();

            sb.Clear();

            if(_health.Medicine.ActiveAgents.Any()){
                foreach(var medicine in _health.Medicine.ActiveAgents){
                    sb.AppendLine($"• {medicine.MedicalGroup.Name} is active");    
                    sb.AppendLine($"  Percent of presence in blood: {medicine.PercentOfPresence:0}%");
                    sb.AppendLine($"  Percent of activity in blood: {medicine.PercentOfActivity:0}%");
                    sb.AppendLine($"  Active doses count: {medicine.ActiveDosesCount}");

                    if(medicine.LastTaken.HasValue){
                        sb.AppendLine($"  Last Taken: {medicine.LastTaken.Value:HH.mm}");
                    }
                }
            } else {
                sb.AppendLine("No medical agents currently active");
            }

            AgentsSummaryText.text = sb.ToString();

            _infoUpdateCounter = 0f;
        }

        #endregion 

    }

    #region IGameController Implementation -- Required by Zara

    public DateTime? WorldTime => _dateTime;

    public IPlayerStatus Player => _player;

    public BodyStatusController Body => _body;

    public IWeatherDescription Weather => _weather;

    public HealthController Health => _health;

    public InventoryController Inventory => _inventory;

    public TimesOfDay TimeOfDay => _timeOfDay;

    #endregion

    #region Controls -- Walking/Running sim

    private bool _isRunning;
    private bool _isWalking;

    public void OnTemperatureValueChanged(Slider e){
        e.gameObject.transform.Find("Value").GetComponent<Text>().text = e.value.ToString("0") + " deg C";

        _weather.SetTemperature(e.value);
    }

    public void OnWindSpeedValueChanged(Slider e){
        e.gameObject.transform.Find("Value").GetComponent<Text>().text = e.value.ToString("0.0") + " m/s";

        _weather.SetWindSpeed(e.value);
    }

    public void OnRainIntensityValueChanged(Slider e){
        e.gameObject.transform.Find("Value").GetComponent<Text>().text = e.value.ToString("0.0");

        _weather.SetRainIntensity(e.value);
    }

    public void OnRunClick(Button e){
        _isWalking = false;
        _isRunning = !_isRunning;

        if(_isRunning)
        {
            e.gameObject.transform.Find("Text").GetComponent<Text>().text = "Stop Running";
            WalkButton.transform.Find("Text").GetComponent<Text>().text = "Start Walking";
        } else {
            e.gameObject.transform.Find("Text").GetComponent<Text>().text = "Start Running";
        }

        _player.SetRunning(_isRunning);
    }

    public void OnWalkClick(Button e){
        _isRunning = false;
        _isWalking = !_isWalking;

        if(_isWalking)
        {
            e.gameObject.transform.Find("Text").GetComponent<Text>().text = "Stop Walking";
            RunButton.transform.Find("Text").GetComponent<Text>().text = "Start Running";
        } else {
            e.gameObject.transform.Find("Text").GetComponent<Text>().text = "Start Walking";
        }

        _player.SetWalking(_isWalking);
    }

    private bool _isUnderwater;
    public void OnUnderwaterClick(Button e){
        _isUnderwater = !_isUnderwater;

        if(_isUnderwater){
            e.gameObject.transform.Find("Text").GetComponent<Text>().text = "Swim Up";
        } else {
            e.gameObject.transform.Find("Text").GetComponent<Text>().text = "Go Underwater";
        }

        _player.SetUnderwater(_isUnderwater);
    }

    #endregion 

    #region Disease/Injury Spawner

    public void OnSpawnDiseaseClick(Dropdown e){
        var diseaseName = e.options[e.value].text?.Replace(" ", "");

        if(string.IsNullOrEmpty(diseaseName))
            return;

        var disease = Activator.CreateInstance(Type.GetType($"ZaraEngine.Diseases.{diseaseName}")) as ZaraEngine.Diseases.DiseaseDefinitionBase;

        if(disease == null)
            return;

        // Blood Poisoning requires a body part (from where blood poisoning has started) to be present. We'll do any body part, does not matter whitch one
        if(diseaseName == "BloodPoisoning"){
            var injuryThatCausedBloodPoisoning = new ZaraEngine.Injuries.ActiveInjury(this, Type.GetType("ZaraEngine.Injuries.DeepCut"), ZaraEngine.Player.BodyParts.RightForearm, WorldTime.Value);

            _health.Status.ActiveInjuries.Add(injuryThatCausedBloodPoisoning);
            _health.Status.ActiveDiseases.Add(new ZaraEngine.Diseases.ActiveDisease(this, disease, injuryThatCausedBloodPoisoning, WorldTime.Value));
        } else {
             // Venom Poisoning requires a body part (from where venom poisoning has started) to be present. We'll do any body part, does not matter whitch one
            if(diseaseName == "VenomPoisoning"){
                var injuryThatCausedVenomPoisoning = new ZaraEngine.Injuries.ActiveInjury(this, Type.GetType("ZaraEngine.Injuries.LightCut"), ZaraEngine.Player.BodyParts.LeftFoot, WorldTime.Value);

                _health.Status.ActiveInjuries.Add(injuryThatCausedVenomPoisoning);
                _health.Status.ActiveDiseases.Add(new ZaraEngine.Diseases.ActiveDisease(this, disease, injuryThatCausedVenomPoisoning, WorldTime.Value));
            } else {
                _health.Status.ActiveDiseases.Add(new ZaraEngine.Diseases.ActiveDisease(this, disease, WorldTime.Value));
            }
        }
    }

    public void OnSpawnInjuryClick(Dropdown e)
    {
        var injuryName = e.options[e.value].text?.Replace(" ", "");
        var bodyPart = (ZaraEngine.Player.BodyParts)e.gameObject.transform.Find("BodyPart").GetComponent<Dropdown>().value;
        var injury = new ZaraEngine.Injuries.ActiveInjury(this, Type.GetType($"ZaraEngine.Injuries.{injuryName}"), bodyPart, WorldTime.Value);

        _health.Status.ActiveInjuries.Add(injury);
    }

    #endregion 

    #region Inventory Demo

    private void RefreshConsumablesUICombo(){
        var index = FirstInventoryItemsList.value;

        FirstInventoryItemsList.ClearOptions();

        foreach (var item in _inventory.Items)
        {
            var medItem = item as InventoryMedicalItemBase;

            if (medItem != null) // it is a medical item of some sort -- pill, solution, syringe
            {
                // This is for demo only: showing in a first list an empty syringe (so we can combine it with med solutions) -- UI limitations let's say ;)
                if (!(medItem is ZaraEngine.Inventory.EmptySyringe))
                {
                    if (medItem.MedicineKind != ZaraEngine.Inventory.InventoryMedicalItemBase.MedicineKinds.Consumable)
                        continue; // do not show in Consumables medical tools like solutions
                }
            }
            else
            {
                if (!item.Type.Contains(ZaraEngine.Inventory.InventoryController.InventoryItemType.Organic))
                    continue; // do not show in Consumables non-organic items (cloth, knife, rope and whatnot)
            }

            var s = $"{item.Count}";

            if (item is ZaraEngine.Inventory.Flask)
                s += $" ({(item as ZaraEngine.Inventory.Flask).DosesLeft} doses)";

            if(item is ZaraEngine.Inventory.Meat){
                var meat = (item as ZaraEngine.Inventory.Meat);
                s += $" ({meat.GetCountNormal(WorldTime.Value)} fr. / {meat.GetCountSpoiled(WorldTime.Value)} sp. pieces)";
            }

            FirstInventoryItemsList.options.Add(new Dropdown.OptionData() { text = $"{item.Name}: {s}" });
        }

        FirstInventoryItemsList.value = index;
    }

    private void RefreshToolsUICombo(){
        var index = SecondInventoryItemsList.value;

        SecondInventoryItemsList.ClearOptions();

        foreach (var item in _inventory.Items)
        {
            var medItem = item as InventoryMedicalItemBase;

            if(medItem != null) // it is a medical item of some sort -- pill, solution, syringe
            {
                if(medItem.MedicineKind == ZaraEngine.Inventory.InventoryMedicalItemBase.MedicineKinds.Consumable)
                    continue; // do not show in Tools medical pills and stuff
            } else {
                if(item.Type.Contains(ZaraEngine.Inventory.InventoryController.InventoryItemType.Organic))
                    continue; // do not show in Tools organic items
            }
            
            SecondInventoryItemsList.options.Add (new Dropdown.OptionData() {text=$"{item.Name}: {item.Count}"});
        }

        SecondInventoryItemsList.value = index;
    }

    private ZaraEngine.Inventory.IInventoryItem GetItemByComboText(string s){
        if(string.IsNullOrEmpty(s))
            return null;

        var a = s.Split(':');
        var itemName = a[0];

        if(string.IsNullOrEmpty(itemName))
            return null;

        return _inventory.Items.FirstOrDefault(x => x.Name.ToLower() == itemName.ToLower()) as ZaraEngine.Inventory.IInventoryItem;
    }

    /* Food/Water/Pills consuming example code */
    public void OnInventoryConsumeClick(Dropdown e){
        var s = e.options[e.value].text;

        var item = GetItemByComboText(s);

        if(item == null)
            return;

        var usageResult = _inventory.TryUse(item, checkOnly: false);

        Debug.Log($"Using the item {item.Name}: {usageResult.Result}");

        RefreshConsumablesUICombo();
    }

    /* Crafting example code */
    public void OnInventoryCombineClick(){
        var s = FirstInventoryItemsList.options[FirstInventoryItemsList.value].text;
        var itemFirst = GetItemByComboText(s);

        s = SecondInventoryItemsList.options[SecondInventoryItemsList.value].text;
        var itemSecond = GetItemByComboText(s);

        if(itemFirst == null || itemSecond == null)
            return;

        // First, we need to know if there are any combinations available with those two items
        var combinatoryResult = ZaraEngine.Inventory.InventoryItemsCombinatoryFactory.Instance.GetMatchedCombinations(itemFirst, itemSecond); // can be any number of items
        var combinationId = Guid.Empty;

        Debug.Log($"{itemFirst.Name} + {itemSecond.Name} = {combinatoryResult.Count} items available to craft. Grabbing the first one (if any).");

        // For this demo, we will take first available combination. IRL, give user a choise
        if(combinatoryResult.Any())
            combinationId = combinatoryResult.First().Id;
        else return;

        // After choosing which combination to use, we need to check for resources availability for it
        var resourcesCheckResult = _inventory.CheckCombinationForResourcesAvailability(combinationId);

        Debug.Log($"{itemFirst.Name} + {itemSecond.Name} = {resourcesCheckResult.Result}");

        if(resourcesCheckResult.Result == InventoryCombinatoryResult.CombinatoryResult.Allowed){
            // Actual crafting time
            var craftResult = _inventory.TryCombine(combinationId, checkOnly: false);

            Debug.Log($"Crafting result for {itemFirst.Name} + {itemSecond.Name} is {(craftResult.ResultedItem?.Name ?? "(nothing)")}");

            if(craftResult.Result == InventoryCombinatoryResult.CombinatoryResult.Allowed){
                var newItem = craftResult.ResultedItem; // item already added to our inventory at this point

                RefreshConsumablesUICombo();
                RefreshToolsUICombo();
            }
        }
    }

    /* Body appliances example code (injections, clothes) */
    public void OnInvenrotyApplyClick(Dropdown e){
        var bodyPart = (ZaraEngine.Player.BodyParts)e.value;
        var s = SecondInventoryItemsList.options[SecondInventoryItemsList.value].text;
        var item = GetItemByComboText(s);

        TryProcessMedicalAppliance(item, bodyPart);
    }

    private bool TryProcessMedicalAppliance(ZaraEngine.Inventory.IInventoryItem item, ZaraEngine.Player.BodyParts bodyPart){
        var appliance = item as ZaraEngine.Inventory.InventoryMedicalItemBase;

        if(item is null)
            return false;

        if(appliance is null){
            Debug.Log($"The item {item.Name} is not a medical item");
            
            return false;
        }

        if(appliance.MedicineKind != ZaraEngine.Inventory.InventoryMedicalItemBase.MedicineKinds.Appliance){
            Debug.Log($"The item {item.Name} is not an appliance");
            
            return false;
        }

        if(!IsApplianceApplicableToBodyPart(appliance, bodyPart)){
            Debug.Log($"The item {item.Name} cannot be applied to {bodyPart}");
            
            return false;
        }

        // After all checks are done, and we know that we can use this appliance on a selected body part, we must use this item in the inventory
        // and notify our health angine that we took some kind of medical thing -- for the health engine to do its job
        var isUsed = false;
        var usageResult = _inventory.TryUse(item, checkOnly: false);

        isUsed = usageResult.Result == ZaraEngine.Inventory.ItemUseResult.UsageResult.UsedAll || usageResult.Result == ZaraEngine.Inventory.ItemUseResult.UsageResult.UsedSingle;

        if (isUsed)
        {
            // Notify the health angine
            _health.OnApplianceTaken(appliance, bodyPart);

            RefreshConsumablesUICombo();
            RefreshToolsUICombo();

            Debug.Log($"Appliance {appliance.Name} was applied to the {bodyPart}");

            return true;
        }

        return false;
    }

    private bool IsApplianceApplicableToBodyPart(ZaraEngine.Inventory.InventoryMedicalItemBase item, ZaraEngine.Player.BodyParts bodyPart)
    {
        if (item.Name == InventoryController.MedicalItems.Splint)
        {
            if (bodyPart != ZaraEngine.Player.BodyParts.LeftForearm &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftShin &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftSpokebone &&
                bodyPart != ZaraEngine.Player.BodyParts.RightForearm &&
                bodyPart != ZaraEngine.Player.BodyParts.RightShin &&
                bodyPart != ZaraEngine.Player.BodyParts.RightSpokebone)

                return false;
        }

        if (item.Name == InventoryController.MedicalItems.Bandage)
        {
            if (bodyPart != ZaraEngine.Player.BodyParts.Belly &&
                bodyPart != ZaraEngine.Player.BodyParts.Forehead &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftBrush &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftChest &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftFoot &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftForearm &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftHip &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftKnee &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftShin &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftShoulder &&
                bodyPart != ZaraEngine.Player.BodyParts.LeftSpokebone &&
                bodyPart != ZaraEngine.Player.BodyParts.RightBrush &&
                bodyPart != ZaraEngine.Player.BodyParts.RightChest &&
                bodyPart != ZaraEngine.Player.BodyParts.RightFoot &&
                bodyPart != ZaraEngine.Player.BodyParts.RightForearm &&
                bodyPart != ZaraEngine.Player.BodyParts.RightHip &&
                bodyPart != ZaraEngine.Player.BodyParts.RightKnee &&
                bodyPart != ZaraEngine.Player.BodyParts.RightShin &&
                bodyPart != ZaraEngine.Player.BodyParts.RightShoulder &&
                bodyPart != ZaraEngine.Player.BodyParts.RightSpokebone &&
                bodyPart != ZaraEngine.Player.BodyParts.Throat)

                return false;
        }

        return true;
    }

    #endregion 

    #region Clothes Demo

    public void OnClothesClick(Button e){
        if(_body.Clothes.Any()){
            // Undress

            _body.Clothes.Clear();

            e.gameObject.transform.Find("Text").GetComponent<Text>().text = "Put On Clothes";
        } else {
            // Dress

            _body.Clothes.Add(_jacket);
            _body.Clothes.Add(_pants);
            _body.Clothes.Add(_hat);
            _body.Clothes.Add(_boots);

            e.gameObject.transform.Find("Text").GetComponent<Text>().text = "Undress";
        }

        // When putting on and off clothes, we need to recalculate the weight of our inventory
        _inventory.RefreshRoughWeight();
    }

    #endregion 

    #region Sleeping Demo

    public void OnSleepClick(Button e){
        var text = e.gameObject.transform.Find("Hours").GetComponent<InputField>().text;

        if(string.IsNullOrEmpty(text))
            return;

        var hours = int.Parse(text);

        if(!_body.Sleep(hours /* how many in-game hours should pass */, 6f /* sleeping will take 6 real seconds */, () => Debug.Log("Woke up!") /* wake up callback */, newTime => _dateTime = newTime /* function to advance game time in big chunks */)){
            Debug.Log("Cannot sleep or not tired");
        }
    }

    public void OnAdvanceTimeClick(){
        _dateTime = _dateTime.AddHours(1);

        // Some food may spoil during this hour
        RefreshConsumablesUICombo();
    }

    #endregion

    #region State Save/Load Demo

    private ZaraEngineState _savedState;

    public void OnStateSaveClick()
    {
        var sw = new System.Diagnostics.Stopwatch();

        sw.Start();

        _savedState = ZaraEngine.EngineState.GetState(this);

        sw.Stop();

        Debug.Log($"Saving took {sw.ElapsedMilliseconds}ms");
    }

    public void OnStateLoadClick()
    {
        if (_savedState == null)
            return;

        var sw = new System.Diagnostics.Stopwatch();

        sw.Start();

        // Important: if you are restoring Zara state asyncronously, be sure not to call .Check() methods on _health and _body during the RestoreState method execution
        // It is not recommended to save or load during the sleep

        ZaraEngine.EngineState.RestoreState(this, _savedState, stateWorldTime => _dateTime = stateWorldTime /* method to restore the saved world time */);

        sw.Stop();

        // We need to reload our demo local variables as well, because after the loading, every inventory item, every disease and injury has been re-created from scratch
        // but we are still storing the old references

        _jacket = _inventory.GetByName(_jacket.Name) as ZaraEngine.Inventory.WaterproofJacket;
        _pants = _inventory.GetByName(_pants.Name) as ZaraEngine.Inventory.WaterproofPants;
        _boots = _inventory.GetByName(_boots.Name) as ZaraEngine.Inventory.RubberBoots;
        _hat = _inventory.GetByName(_hat.Name) as ZaraEngine.Inventory.LeafHat;

        RefreshConsumablesUICombo();
        RefreshToolsUICombo();

        Debug.Log($"Loading took {sw.ElapsedMilliseconds}ms");
    }

    #endregion 

}
