using Godot;
using System;
using ZaraEngine;
using ZaraEngine.HealthEngine;
using ZaraEngine.Inventory;
using ZaraEngine.Player;

public partial class PlayerNode : Node2D, IGameController
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

	private readonly RandomNumberGenerator _random;

	#endregion

	public PlayerNode()
	{
		_random = new RandomNumberGenerator();

		_random.Randomize();
	}

	public override void _Ready()
	{
		/* Zara Initialization code start =======>> */

		ZaraEngine.Helpers.InitializeRandomizer(_random.RandfRange);

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
		_weather.SetTemperature(21f);
		_weather.SetWindSpeed(10f);
		_weather.SetRainIntensity(0.12f);

		#endregion
	}

	public override void _Process(double delta)
	{
		/* These two calls are required by Zara */

		_body.Check((float)delta);
		_health.Check((float)delta);

		// Progress our game time
		_dateTimeCounter += (float)delta;

		if (_dateTimeCounter >= 0.05f)
		{
			_dateTime = _dateTime.AddSeconds(_dateTimeCounter * 10f); // in-game time is 10x the real one
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
