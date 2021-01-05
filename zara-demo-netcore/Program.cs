using System;
using System.Diagnostics;

namespace ZaraEngine.NetCore.Demo
{
    class Program
    {
        // Declare two persons
        private static Person _person1 = null;
        private static Person _person2 = null;
        
        // Will be used for Zara random range function 
        private static System.Random _random;

        // Trepliev will be in a normal climate
        private static WeatherDescription _weatherForTrepliev;

        // For Freud we'll set up desert climate
        private static WeatherDescription _weatherForFreud;

        // Declare our "game loop"
        private static System.Threading.Thread _loop;

        // Counter for progressing the game time
        private static float _dateTimeCounter;

        // Our game time itself
        private static DateTime _dateTime;

        // Our in-game time of day
        private static TimesOfDay _timeOfDay;

        static void Main(string[] args)
        {
            // Initialize Zara randomizer
            _random = new Random(DateTime.Now.Millisecond);
            ZaraEngine.Helpers.InitializeRandomizer((a, b) => (float)(a + ((b-a) * _random.NextDouble())));
            
            // Initialize our weather
            _weatherForTrepliev = new WeatherDescription();
            _weatherForTrepliev.SetTemperature(27f);
            _weatherForTrepliev.SetWindSpeed(0.1f);
            _weatherForTrepliev.SetRainIntensity(0.15f);

            _weatherForFreud = new WeatherDescription();
            _weatherForFreud.SetTemperature(57f);
            _weatherForFreud.SetWindSpeed(5.0f);
            _weatherForFreud.SetRainIntensity(0f);

            // Initialize our in-game time
            _dateTime = DateTime.Now;
            _timeOfDay = TimesOfDay.Evening;

            // Initialize our persons. They will share the same game time and time of day
            Func<DateTime?> gameTimeFunc = () => _dateTime;
            Func<TimesOfDay> timeOfDayFunc = () => _timeOfDay;

            _person1 = InitialzeNewPerson("Trepliev", _weatherForTrepliev, gameTimeFunc, timeOfDayFunc);
            _person2 = InitialzeNewPerson("Freud", _weatherForFreud, gameTimeFunc, timeOfDayFunc);

            Console.WriteLine("Zara is running");

            // Start the "game loop"
            _loop = new System.Threading.Thread(LoopThread, 0);
            _loop.Start();
        }

        private static Person InitialzeNewPerson(string name, WeatherDescription weather, Func<DateTime?> gameTimeFunc, Func<TimesOfDay> timeOfDayFunc){
            var p = new Person(name);

            p.Initialize(weather, gameTimeFunc, timeOfDayFunc);

            return p;
        }

        // Our "game loop"
        private static void LoopThread(object state)
        {
            var deltaTime = 0f;
            var sw = new Stopwatch();
            
            // Update warmth levels right away, so on a first "frame" we already have one calculated
            _person1.Body.UpdateWarmthLevelCacheImmediately();
            _person2.Body.UpdateWarmthLevelCacheImmediately();
            
            while (true)
            {
                sw.Start();

                // Update our persons
                _person1.Update(deltaTime);
                _person2.Update(deltaTime);

                // Progress our in-game time. We'll not change time of day to make everything simplier
                _dateTimeCounter += deltaTime;

                if (_dateTimeCounter > 0.05f)
                {
                    _dateTime = _dateTime.AddSeconds(0.5d); // in-game time is 10x the real one
                    _dateTimeCounter = 0f;
                }

                // Cap the "framerate" a little
                System.Threading.Thread.Sleep(5);
                
                sw.Stop();

                // Calculate our delta time
                deltaTime = sw.ElapsedMilliseconds / 1000f;
                
                sw.Reset();
            }
        }
    }
}
