using System;
using ZaraEngine;
using ZaraEngine.Player;
using ZaraEngine.Inventory;
using ZaraEngine.HealthEngine;

// It's our "GameController"
public class Person : IPerson
{

    private PlayerStatus _player;

    private BodyStatusController _body;

    private WeatherDescription _weather;

    private HealthController _health;

    private InventoryController _inventory;

    private Func<DateTime?> _getDateTimeFunc;

    private Func<TimesOfDay> _getTimeOfDayFunc;
  
    public void Initialize(WeatherDescription weather, Func<DateTime?> getDateTimeFunc, Func<TimesOfDay> getTimeOfDayFunc)
    {
        _getDateTimeFunc = getDateTimeFunc;
        _getTimeOfDayFunc = getTimeOfDayFunc;

        /* Zara Initialization code start =======>> */

        _health = new HealthController(this);
        _body = new BodyStatusController(this);
        _weather = weather;
        _player = new PlayerStatus();
        _inventory = new InventoryController(this);
        _body.Initialize();
        _health.Initialize();

        /* <<======= Zara Initialization code end */
    }

    public void Update(float deltaTime)
    {
        /* These two calls are required by Zara */
        
        _body.Check(deltaTime);
        _health.Check(deltaTime);
    }

    #region IGameController Implementation -- Required by Zara

    public DateTime? WorldTime => _getDateTimeFunc();

    public IPlayerStatus Player => _player;

    public BodyStatusController Body => _body;

    public IWeatherDescription Weather => _weather;

    public HealthController Health => _health;

    public InventoryController Inventory => _inventory;

    public TimesOfDay TimeOfDay => _getTimeOfDayFunc();

    #endregion

}
