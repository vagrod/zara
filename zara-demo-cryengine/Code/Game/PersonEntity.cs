using CryEngine.UI;
using CryEngine.UI.Components;
using System;
using System.Linq;
using System.Text;

namespace CryEngine.Projects.Game
{
    [EntityComponent]
    public class PersonEntity : EntityComponent
    {

        private const float StatsUpdateInterval = 1f; // real seconds

        private Person _person;
        private DateTime _worldTime;

        private float _worldTimeCounter;
        private float _demoTextUpdateCounter;

        private Text _healthText;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            var weather = new WeatherDescription();

            weather.SetRainIntensity(0.08f);
            weather.SetWindSpeed(30f);
            weather.SetTemperature(25f);

            _worldTime = DateTime.Now;

            _person = new Person();
            _person.Initialize(weather, () => _worldTime, () => ZaraEngine.TimesOfDay.Evening); // will always be evening just to make things simplier here

            CreateDemoOutput();
        }

        private void CreateDemoOutput()
        {
            // Canvas to contain stats label.
            var canvas = SceneObject.Instantiate<Canvas>(SceneManager.RootObject);

            // Create stats Label.
            _healthText = canvas.AddComponent<Text>();
            _healthText.Alignment = Alignment.TopLeft;
            _healthText.Height = 18;
            _healthText.Offset = new Point(10, 60);
            _healthText.Color = Color.Black.WithAlpha(0.5f);
        }

        protected override void OnUpdate(float frameTime)
        {
            base.OnUpdate(frameTime);

            if(_worldTimeCounter >= 0.05f)
            {
                // World time is 10x the real time
                _worldTime = _worldTime.AddSeconds(_worldTimeCounter * 10f);

                _worldTimeCounter = 0f;
            }

            _worldTimeCounter += frameTime;

            if(_demoTextUpdateCounter >= StatsUpdateInterval)
            {
                var sb = new StringBuilder();
                var stats = _person.Health.Status;

                // Collect the demo stats
                sb.AppendLine("Vitals:");
                sb.AppendLine($"  • Body Temp.: {stats.BodyTemperature:0.0} deg C");
                sb.AppendLine($"  • Heart Rate: {stats.HeartRate:0} bpm");
                sb.AppendLine($"  • Blood Pressure: {stats.BloodPressureTop:0} / {stats.BloodPressureBottom:0}");
                sb.AppendLine($"  • Food Level: {stats.FoodPercentage:0.0} %");
                sb.AppendLine($"  • Water Level: {stats.WaterPercentage:0.0} %");
                sb.AppendLine($"  • Blood Level: {stats.BloodPercentage:0.0} %");
                sb.AppendLine($"  • Blood Loss? {(stats.IsBloodLoss ? "yes" : "no")}");
                sb.AppendLine($"  • Stamina Level: {stats.StaminaPercentage:0.0} %");
                sb.AppendLine($"  • Fatigue: {stats.FatiguePercentage:0.0} %");
                sb.AppendLine($"  • Oxygen Level: {stats.OxygenPercentage:0.0} %");
                sb.AppendLine($"  • Wetness Level: {_person.Body.WetnessLevel:0.0} %");

                sb.AppendLine();
                sb.AppendLine("Stats:");
                sb.AppendLine($"  • World time is {_worldTime:MMMM dd, HH:mm:ss}");
                sb.AppendLine($"  • Last Time Slept: {stats.LastSleepTime:MMMM dd, HH:mm}                            ."); // without these spaces text is cut. I guess, some cryengine weirdness :)
                sb.AppendLine($"  • Last Health Update {stats.CheckTime:MMMM dd, HH:mm:ss}");

                sb.AppendLine();
                sb.AppendLine("Diseases / Injuries:");
                sb.AppendLine($"  • Has active diseases? {(stats.ActiveDiseases.Any() ? "yes" : "no")}");
                sb.AppendLine($"  • Has active Injuries? {(stats.ActiveInjuries.Any() ? "yes" : "no")}");

                _healthText.Content = sb.ToString();

               _demoTextUpdateCounter = 0f;
            }

            _demoTextUpdateCounter += frameTime;

            // Update Zara instance
            _person.Update(frameTime);
        }

    }
}
