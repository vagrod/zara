using System;
using FlaxEngine;
using ZaraEngine;
using ZaraEngine.HealthEngine;
using ZaraEngine.Inventory;
using ZaraEngine.Player;

namespace Game.Game
{
    public class PlayerActor : Script, IGameController
    {

        private DateTime _dateTime;

        private PlayerStatus _player;

        private BodyStatusController _body;

        private WeatherDescription _weather;

        private HealthController _health;

        private InventoryController _inventory;

        private TimesOfDay _timeOfDay;

        #region Demo app fields

        private float _dateTimeCounter;

        // Will be used for Zara random range function 
        private static System.Random _random;

        #endregion

        public override void OnStart()
        {
            /* Zara Initialization code start =======>> */

            // Initialize Zara randomizer
            _random = new Random(DateTime.Now.Millisecond);
            ZaraEngine.Helpers.InitializeRandomizer((a, b) => (float)(a + ((b - a) * _random.NextDouble())));

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

            #region Demo App Init

            // Defaults
            _weather.SetTemperature(24f);
            _weather.SetWindSpeed(8f);
            _weather.SetRainIntensity(0.06f);

            #endregion
        }

        public override void OnUpdate()
        {
            /* These two calls are required by Zara */

            _body.Check(Time.DeltaTime);
            _health.Check(Time.DeltaTime);

            // Progress our game time
            _dateTimeCounter += Time.DeltaTime;

            if (_dateTimeCounter >= 0.05f)
            {
                _dateTime = _dateTime.AddSeconds(_dateTimeCounter*10f); // in-game time is 10x the real one
                _dateTimeCounter = 0f;
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

    }
}
