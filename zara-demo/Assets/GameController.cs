using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZaraEngine;
using ZaraEngine.Player;
using ZaraEngine.Inventory;
using ZaraEngine.HealthEngine;
using System.Text;
using System.Runtime.InteropServices;

public class GameController : MonoBehaviour, IGameController
{

    private DateTime _dateTime;

    private PlayerStatus _player;

    private BodyStatusController _body;

    private WeatherDescription _weather;

    private HealthController _health;

    private InventoryController _inventory;

    private TimesOfDay _timeOfDay;

    private float _infoUpdateCounter;
    private float _dateTimeCounter;

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

    public Dropdown ConsumablesList;

    #endregion 

    // Start is called before the first frame update
    void Start()
    {
        /* Zara Initialization code start =======>> */

        ZaraEngine.Helpers.InitializeRandomizer((a, b) => UnityEngine.Random.Range(a, b));

        _dateTime = DateTime.Now;
        _timeOfDay = TimesOfDay.Evening;
        _health = new HealthController(this);
        _body = new BodyStatusController(this);
        _weather = new WeatherDescription();
        _player = new PlayerStatus();
        _inventory = new InventoryController(this);

        ClothesGroups.Initialize(this);

        _body.Initialize();
        _health.Initialize();

        /* <<======= Zara Initialization code end */

        var flaskWithWater = new ZaraEngine.Inventory.Flask();

        flaskWithWater.FillUp(WorldTime.Value);
        //flaskWithWater.Disinfect(WorldTime.Value);

        // Let's add some items to the inventory to play with in this demo
        _inventory.AddItem(new ZaraEngine.Inventory.Cloth { Count = 20 });
        _inventory.AddItem(flaskWithWater);
        _inventory.AddItem(new ZaraEngine.Inventory.Meat { Count = 3 });
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

        RefreshConsumablesUICombo();

        // Defaults
        _weather.SetTemperature(27f);
        _weather.SetWindSpeed(0.1f);
        _weather.SetRainIntensity(0f);
    }

    // Update is called once per frame
    void Update()
    {
        /* These two calls are required by Zara */
        
        _body.Check(Time.deltaTime);
        _health.Check(Time.deltaTime);

        /* Updating UI info */

        _infoUpdateCounter += Time.deltaTime;
        _dateTimeCounter += Time.deltaTime;

        if(_dateTimeCounter > 0.05f){
            _dateTime = _dateTime.AddSeconds(0.5d); // in-game time is 10x the real one
            _dateTimeCounter = 0f;
        }

        if (_infoUpdateCounter >= 1f)
        {
            BodyTempText.text =  $"Body Temp.: {_health.Status.BodyTemperature.ToString("#0.0")} deg C";
            BloodPressureText.text =  $"Blood Pressure: {_health.Status.BloodPressureTop.ToString("000")}/{_health.Status.BloodPressureBottom.ToString("#00")}";
            FatigueLevelText.text =  $"Fatigue: {_health.Status.FatiguePercentage.ToString("#0.0")}%";
            HeartRateText.text =  $"Heart Rate: {_health.Status.HeartRate.ToString("00")}bpm";
            FoodLevelText.text =  $"Foood Level: {_health.Status.FoodPercentage.ToString("#0.0")}%";
            WaterLevelText.text =  $"Water Level: {_health.Status.WaterPercentage.ToString("#0.0")}%";
            WetnessLevelText.text =  $"Is Wet? {(_body.IsWet ? "yes" : "no")} (Wetness Level is {_body.WetnessLevel.ToString("#0.0")}%)";
            WarmthLevelText.text = $"Warmth Score is {_body.GetWarmthLevel().ToString("0.0")} [-5..+5 is a comfort warm feel]";
            BloodLevelText.text =  $"Blood Level: {_health.Status.BloodPercentage.ToString("#0.0")}% (Blood Loss? {(_health.Status.IsBloodLoss ? "yes" : "no")})";
            StaminaText.text =  $"Stamina Level: {_health.Status.StaminaPercentage.ToString("#0.0")}%";
            OxygenLevelText.text =  $"Oxygen Level: {_health.Status.OxygenPercentage.ToString("#0.0")}%";

            LastSleepTimeText.text =  $"Last Time Slept: {_health.Status.LastSleepTime.ToString("MMMM dd, HH:mm")}";
            LastHealthCheckTimeText.text =  $"Last Health Update: {_health.Status.CheckTime.ToString("MMMM dd, HH:mm:ss")}";

            CanEatText.text =  $"Can Eat? {(_health.Status.IsFoodDisgust ? "no" : "yes")}";
            CanSleepText.text =  $"Has Sleep Disorder? {(_health.Status.IsSleepDisorder ? "yes" : "no")}";
            CanRunText.text =  $"Can Run? {(_health.Status.CannotRun ? "no" : "yes")}";
            HasLegFractureText.text =  $"Has Leg Fracture? {(_health.Status.IsLegFracture ? "yes" : "no")}";

            var sb = new StringBuilder();

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
                    sb.AppendLine($"\t  Active. Current stage is {st.Level}, stage will end at {(st.WillEndAt.HasValue ? st.WillEndAt.Value.ToString("HH:mm") : "n/a")}");
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
                    sb.AppendLine($"\t  Scheduled for {(inj.InjuryTriggerTime.ToString("HH:mm"))}");
                } else {
                    // Active Injury
                    if(st.SelfHealAt.HasValue){
                        sb.AppendLine($"\t  Active. Current stage is {st.Level}, will self-heal at {st.SelfHealAt.Value.ToString("HH:mm")}");
                    } else
                    {
                        sb.AppendLine($"\t  Active. Current stage is {st.Level}, stage will end at {(st.WillEndAt.HasValue ? st.WillEndAt.Value.ToString("HH:mm") : "n/a")}");
                    }
                }

                if(st != null)
                    sb.AppendLine($"\t  Is treatment needed? {(st.SelfHealAt.HasValue ? "no" : "yes")}");

                sb.AppendLine();
            }

            InjuriesInfoText.text = sb.ToString();

            _infoUpdateCounter = 0f;
        }
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

    #region Controls

    private bool _isRunning;
    private bool _isWalking;

    public void OnTemperatureValueChanged(Slider e){
        e.gameObject.transform.Find("Value").GetComponent<Text>().text = e.value.ToString("0") + " deg C";

        _weather.SetTemperature(e.value);
    }

    public void OnWindSpeedValueChanged(Slider e){
        e.gameObject.transform.Find("Value").GetComponent<Text>().text = e.value.ToString("0.0");

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
            var injuryThatCausedBloodPoisoning = new ZaraEngine.Injuries.ActiveInjury(this, Type.GetType("ZaraEngine.Injuries.DeepCut"), ZaraEngine.Injuries.BodyParts.RightForearm, WorldTime.Value);

            _health.Status.ActiveInjuries.Add(injuryThatCausedBloodPoisoning);
            _health.Status.ActiveDiseases.Add(new ZaraEngine.Diseases.ActiveDisease(this, disease, injuryThatCausedBloodPoisoning, WorldTime.Value));
        } else {
             // Venom Poisoning requires a body part (from where venom poisoning has started) to be present. We'll do any body part, does not matter whitch one
            if(diseaseName == "VenomPoisoning"){
                var injuryThatCausedVenomPoisoning = new ZaraEngine.Injuries.ActiveInjury(this, Type.GetType("ZaraEngine.Injuries.LightCut"), ZaraEngine.Injuries.BodyParts.LeftFoot, WorldTime.Value);

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
        var bodyPart = (ZaraEngine.Injuries.BodyParts)e.gameObject.transform.Find("BodyPart").GetComponent<Dropdown>().value;
        var injury = new ZaraEngine.Injuries.ActiveInjury(this, Type.GetType($"ZaraEngine.Injuries.{injuryName}"), bodyPart, WorldTime.Value);

        _health.Status.ActiveInjuries.Add(injury);
    }

    #endregion 

    #region Consumables

    private void RefreshConsumablesUICombo(){
        ConsumablesList.ClearOptions();

        foreach (var item in _inventory.Items)
        {
            if(item is ZaraEngine.Inventory.InventoryConsumableItemBase)
                ConsumablesList.options.Add (new Dropdown.OptionData() {text=$"{item.Name}: {item.Count}"});

        }
    }

    public void OnConsumeClick(Dropdown e){
        var s = e.options[e.value].text;

        if(string.IsNullOrEmpty(s))
            return;

        var a = s.Split(':');
        var itemName = a[0];

        if(string.IsNullOrEmpty(itemName))
            return;

        var item = _inventory.Items.FirstOrDefault(x => x.Name.ToLower() == itemName.ToLower());

        if(item == null)
            return;

        var usageResult = _inventory.TryUse(item, false);

        Debug.Log($"Using the item {itemName}: {usageResult.Result}");

        var index = e.value;

        RefreshConsumablesUICombo();

        e.value = index;
    }

    #endregion 

}
