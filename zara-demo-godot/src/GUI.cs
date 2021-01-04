using Godot;
using System.Linq;
using System.Text;

public class GUI : Node2D
{

    private const float StatsUpdateInterval = 1f; // real seconds

    private PlayerNode _player;
    private bool _isReady;

    private float _statsUpdateCounter;

    private Label _vitalsData;
    private Label _statsData;
    private Label _diseasesData;

    public override void _Ready()
    {
        _vitalsData = GetNode<Label>("GUI/Holder/Left-column/Vitals-data");
        _statsData = GetNode<Label>("GUI/Holder/Left-column/Stats-data");
        _diseasesData = GetNode<Label>("GUI/Holder/Left-column/Diseases-data");
    }

    public void SetPlayer(PlayerNode player)
    {
        _player = player;
        _isReady = true;
    }

    public override void _Process(float delta)
    {
        if (!_isReady) return;

        _statsUpdateCounter += delta;

        if (_statsUpdateCounter >= StatsUpdateInterval)
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

            _vitalsData.Text = sb.ToString();

            sb.Clear();

            sb.AppendLine($"  - World time is {_player.WorldTime:MMMM dd, HH:mm:ss}");
            sb.AppendLine($"  - Last Time Slept: {stats.LastSleepTime:MMMM dd, HH:mm} ");
            sb.AppendLine($"  - Last Health Update: {stats.CheckTime:MMMM dd, HH:mm:ss}");

            _statsData.Text = sb.ToString();

            sb.Clear();

            sb.AppendLine($"  - Has active diseases? {(stats.ActiveDiseases.Any() ? "yes" : "no")}");
            sb.AppendLine($"  - Has active Injuries? {(stats.ActiveInjuries.Any() ? "yes" : "no")}");

            _diseasesData.Text = sb.ToString();
        }

    }
}
