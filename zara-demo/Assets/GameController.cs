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

    private DateTime? _worldTime;

    private IPlayerStatus _player;

    private BodyStatusController _body;

    private IWeatherDescription _weather;

    private HealthController _health;

    private InventoryController _inventory;

    private TimesOfDay _timeOfDay;

    #region UI elements for data display

    public Text BodyTempText;

    #endregion 

    // Start is called before the first frame update
    void Start()
    {
        _worldTime = DateTime.Now;
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
        _body.Check();
        _health.Check(Time.deltaTime);

        BodyTempText.text = _health.Status.BodyTemperature.ToString("00.0");
    }

    #region IGameController Implementation

    public DateTime? WorldTime => _worldTime;

    public IPlayerStatus Player => _player;

    public BodyStatusController Body => _body;

    public IWeatherDescription Weather => _weather;

    public HealthController Health => _health;

    public InventoryController Inventory => _inventory;

    public TimesOfDay TimeOfDay => _timeOfDay;

    #endregion

}
