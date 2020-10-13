using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZaraEngine;
using ZaraEngine.Player;
using ZaraEngine.Inventory;
using ZaraEngine.HealthEngine;

public class GameController : MonoBehaviour, IGameController
{

    private IPlayerStatus _player;

    private BodyStatusController _body;

    private IWeatherDescription _weather;

    private HealthController _health;

    private InventoryController _inventory;

    private TimesOfDay _timeOfDay;

    private float _infoUpdateCounter;

    #region UI elements for data display

    public Text BodyTempText;
    public Text BloodPressureText;
    public Text FatigueLevelText;
    public Text HeartRateText;
    public Text FoodLevelText;
    public Text WaterLevelText;
    public Text BloodLevelText;
    public Text StaminaText;

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

    #endregion 

    // Start is called before the first frame update
    void Start()
    {
        ZaraEngine.Helpers.InitializeRandomizer((a, b) => UnityEngine.Random.Range(a, b));

        _timeOfDay = TimesOfDay.Evening;
        _health = new HealthController(this);
        _body = new BodyStatusController(this);
        _weather = new WeatherDescription();
        _player = new PlayerStatus();
        _inventory = new InventoryController(this);

        ClothesGroups.Initialize(this);

        _body.Initialize();
        _health.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        _body.Check(Time.deltaTime);
        _health.Check(Time.deltaTime);

        _infoUpdateCounter += Time.deltaTime;

        if (_infoUpdateCounter >= 1f)
        {
            BodyTempText.text =  $"Body Temp.: {_health.Status.BodyTemperature.ToString("00.0")} deg C";
            BloodPressureText.text =  $"Blood Pressure: {_health.Status.BloodPressureTop.ToString("000")}/{_health.Status.BloodPressureBottom.ToString("#00")}";
            FatigueLevelText.text =  $"Fatigue: {_health.Status.FatiguePercentage.ToString("00.0")}%";
            HeartRateText.text =  $"Heart Rate: {_health.Status.HeartRate.ToString("00")}bpm";
            FoodLevelText.text =  $"Foood Level: {_health.Status.FoodPercentage.ToString("00.0")}%";
            WaterLevelText.text =  $"Water Level: {_health.Status.WaterPercentage.ToString("00.0")}%";
            WetnessLevelText.text =  $"Is Wet? {(_body.IsWet ? "yes" : "no")} (Wetness Level is {_body.WetnessLevel.ToString("00.0")}%)";
            WarmthLevelText.text = $"Warmth Score is {_body.GetWarmthLevel().ToString("0.0")} [-5..+5 is a comfort warm feel]";
            BloodLevelText.text =  $"Blood Level: {_health.Status.BloodPercentage.ToString("00.0")}% (Blood Loss? {(_health.Status.IsBloodLoss ? "yes" : "no")})";
            StaminaText.text =  $"Stamina Level: {_health.Status.StaminaPercentage.ToString("00.0")}%";

            LastSleepTimeText.text =  $"Last Time Slept: {_health.Status.LastSleepTime.ToString("MMMM dd, HH:mm")}";
            LastHealthCheckTimeText.text =  $"Last Health Update: {_health.Status.CheckTime.ToString("MMMM dd, HH:mm:ss")}";

            CanEatText.text =  $"Can Eat? {(_health.Status.IsFoodDisgust ? "no" : "yes")}";
            CanSleepText.text =  $"Has Sleep Disorder? {(_health.Status.IsSleepDisorder ? "yes" : "no")}";
            CanRunText.text =  $"Can Run? {(_health.Status.CannotRun ? "no" : "yes")}";
            HasLegFractureText.text =  $"Has Leg Fracture? {(_health.Status.IsLegFracture ? "yes" : "no")}";

            DiseasesInfoText.text = $"Has Active Disease: {(_health.Status.IsActiveDisease ? "yes" : "no")}";

            InjuriesInfoText.text = $"Has Active Injury: {(_health.Status.IsActiveInjury ? "yes" : "no")}";

            _infoUpdateCounter = 0f;
        }
    }

    #region IGameController Implementation

    public DateTime? WorldTime => DateTime.Now;

    public IPlayerStatus Player => _player;

    public BodyStatusController Body => _body;

    public IWeatherDescription Weather => _weather;

    public HealthController Health => _health;

    public InventoryController Inventory => _inventory;

    public TimesOfDay TimeOfDay => _timeOfDay;

    #endregion

}
