using System;
using System.Linq;
using ZaraEngine;
using ZaraEngine.Player;
using ZaraEngine.Inventory;
using ZaraEngine.HealthEngine;
using System.Text;
using ZaraEngine.StateManaging;

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

    private float _infoUpdateCounter;
    private float _dateTimeCounter;

    // our clothes references
    private ZaraEngine.Inventory.WaterproofJacket _jacket;
    private ZaraEngine.Inventory.WaterproofPants _pants;
    private ZaraEngine.Inventory.RubberBoots _boots;
    private ZaraEngine.Inventory.LeafHat _hat;

    #endregion 

    private System.Threading.Thread _loop;

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

        // Let's add some items to the inventory to play with in this demo

        var flaskWithWater = new ZaraEngine.Inventory.Flask();

        flaskWithWater.FillUp(WorldTime.Value);
        //flaskWithWater.Disinfect(WorldTime.Value);

        _jacket = new ZaraEngine.Inventory.WaterproofJacket();
        _pants = new ZaraEngine.Inventory.WaterproofPants();
        _boots = new ZaraEngine.Inventory.RubberBoots();
        _hat = new ZaraEngine.Inventory.LeafHat();

        _inventory.AddItem(flaskWithWater);

        _inventory.AddItem(_jacket);
        _inventory.AddItem(_pants);
        _inventory.AddItem(_boots);
        _inventory.AddItem(_hat);

        var meat = new ZaraEngine.Inventory.Meat { Count = 1 };

        // We just gathered two of Meat. If will spoil in MinutesUntilSpoiled game minutes
        meat.AddGatheringInfo(WorldTime.Value, 2);

        _inventory.AddItem(new ZaraEngine.Inventory.Cloth { Count = 20 });
        _inventory.AddItem(meat);
        _inventory.AddItem(new ZaraEngine.Inventory.AntisepticSponge { Count = 5 });
        _inventory.AddItem(new ZaraEngine.Inventory.Bandage { Count = 5 });
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
        _inventory.AddItem(new ZaraEngine.Inventory.DisinfectingPellets { Count = 5 });

        // Defaults
        _weather.SetTemperature(27f);
        _weather.SetWindSpeed(0.1f);
        _weather.SetRainIntensity(0f);

        #endregion

        _loop = new System.Threading.Thread(LoopThread, 0);
        _loop.Start();
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

        #region Demo App Output

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
