using System;
using ZaraEngine;
using ZaraEngine.Player;
using ZaraEngine.Inventory;
using ZaraEngine.HealthEngine;

public class GameController : IGameController
{

    private DateTime _dateTime;

    private PlayerStatus _player;

    private BodyStatusController _body;

    private WeatherDescription _weather;

    private HealthController _health;

    private InventoryController _inventory;

    private TimesOfDay _timeOfDay;

    private System.Random _random;

    #region Demo app fields

    private System.Threading.Thread _loop;
    private float _infoUpdateCounter;
    private float _dateTimeCounter;

    #endregion 

    public void Initialize()
    {
        _random = new Random(DateTime.Now.Millisecond);

        /* Zara Initialization code start =======>> */

        ZaraEngine.Helpers.InitializeRandomizer((a, b) => (float)(a + ((b-a) * _random.NextDouble())));

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

        // Defaults
        _weather.SetTemperature(27f);
        _weather.SetWindSpeed(0.1f);
        _weather.SetRainIntensity(0f);

        // Start the "game loop"
        _loop = new System.Threading.Thread(LoopThread, 0);
        _loop.Start();

        #endregion
    }

    private void LoopThread(object state)
    {
        var time = DateTime.Now;

        while(true)
        {
            Update((float)(DateTime.Now - time).TotalSeconds);

            time = DateTime.Now;

            // Cap the "framerate"
            System.Threading.Thread.Sleep(33);
        } 
    }

    private void Update(float deltaTime)
    {
        /* These two calls are required by Zara */
        
        _body.Check(deltaTime);
        _health.Check(deltaTime);

        #region Demo App: Advancing the Time and displaying Output

        _infoUpdateCounter += deltaTime;
        _dateTimeCounter += deltaTime;

        if (_dateTimeCounter > 0.05f)
        {
            _dateTime = _dateTime.AddSeconds(0.5d); // in-game time is 10x the real one
            _dateTimeCounter = 0f;
        }

        if (_infoUpdateCounter >= 1f)
        {
            _infoUpdateCounter = 0f;

            Console.WriteLine($"Health: body temp. is {_health.Status.BodyTemperature} deg C");
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

}
