using FlaxEngine;
using FlaxEngine.GUI;
using Game.Game;
using System.Text;
using System.Linq;

public class Overlay : Script
{

    private const float StatsUpdateInterval = 1f; // real seconds

    public UIControl VitalsData;
    public UIControl StatsData;
    public UIControl DiseasesData;

    private Label _vitalsLabel;
    private Label _statsLabel;
    private Label _diseasesLabel;

    private PlayerActor _player;

    private float _statsUpdateCounter;

    public override void OnStart()
    {
        _vitalsLabel = (Label)VitalsData.Control;
        _statsLabel = (Label)StatsData.Control;
        _diseasesLabel = (Label)DiseasesData.Control;

        _player = Scene.FindActor("Player").GetScript<PlayerActor>();
    }

    public override void OnUpdate()
    {
        _statsUpdateCounter += Time.DeltaTime;

        if(_statsUpdateCounter >= StatsUpdateInterval)
        {
            _statsUpdateCounter = 0f;

            var sb = new StringBuilder();
            var stats = _player.Health.Status;

            // Collect the demo stats
            sb.AppendLine($"  - Body Temp.: {stats.BodyTemperature:0.0} deg C");
            sb.AppendLine($"  - Heart Rate: {stats.HeartRate:0} bpm");
            sb.AppendLine($"  - Blood Pressure: {stats.BloodPressureTop:0} / {stats.BloodPressureBottom:0}");
            sb.AppendLine($"  - Food Level: {stats.FoodPercentage:0.0} %");
            sb.AppendLine($"  - Water Level: {stats.WaterPercentage:0.0} %");
            sb.AppendLine($"  - Blood Level: {stats.BloodPercentage:0.0} %");
            sb.AppendLine($"  - Blood Loss? {(stats.IsBloodLoss ? "yes" : "no")}");
            sb.AppendLine($"  - Stamina Level: {stats.StaminaPercentage:0.0} %");
            sb.AppendLine($"  - Fatigue: {stats.FatiguePercentage:0.0} %");
            sb.AppendLine($"  - Oxygen Level: {stats.OxygenPercentage:0.0} %");
            sb.AppendLine($"  - Wetness Level: {_player.Body.WetnessLevel:0.0} %");
            sb.AppendLine($"  - Warmth Level: {_player.Body.WarmthLevelCached:0.0} (-5..+5 is a comfort zone)");

            _vitalsLabel.Text = sb.ToString();

            sb.Clear();

            sb.AppendLine($"  - World time is {_player.WorldTime:MMMM dd, HH:mm:ss}");
            sb.AppendLine($"  - Last Time Slept: {stats.LastSleepTime:MMMM dd, HH:mm} ");
            sb.AppendLine($"  - Last Health Update: {stats.CheckTime:MMMM dd, HH:mm:ss}");

            _statsLabel.Text = sb.ToString();

            sb.Clear();

            sb.AppendLine($"  - Has active diseases? {(stats.ActiveDiseases.Any() ? "yes" : "no")}");
            sb.AppendLine($"  - Has active Injuries? {(stats.ActiveInjuries.Any() ? "yes" : "no")}");

            _diseasesLabel.Text = sb.ToString();
        }
    }

}