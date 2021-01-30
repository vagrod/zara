using System;
using System.Linq;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Player
{
    public class WetnessController : IAcceptsStateChange
    {

        public const float HotTemperature    = 30;  // C (and higher)
        public const float NormalTemperature = 20;  // C (and higher)
        public const float ColdTemperature   = 10;  // C (and higher)
        public const float FreezeTemperature = -80; // C (and higher)

        private const float WindSpeedForMaxDrying = 7f; // m/s
        private const float MaxWindDryingRate = 0.422f;  // percent per real second
        
        public const float HotDryRate    = 0.75f;   // percent per real second
        public const float NormalDryRate = 0.325f;  // percent per real second
        public const float ColdDryRate   = 0.11f;   // percent per real second
        public const float FreezeDryRate = 0.065f;  // percent per real second

        public const float RainWetnessGainRate = 1.93f; // max percent per real second

        public bool IsWet { get; private set; }

        public float WetnessLevel
        {
            get { return _wetnessValue; }
        }

        private readonly IGameController _gc;
        private DateTime? _lastGettingWetTime;
        private float _wetnessValue;

        public WetnessController(IGameController gc)
        {
            _gc = gc;
        }

        public void Initialize()
        {

        }

        public void Check(float deltaTime)
        {
            CheckForRain(deltaTime);

            if (CheckForUnderwater())
                return;

            if (!_lastGettingWetTime.HasValue)
                return;

            CheckForDrying(deltaTime);
        }

        private void CheckForRain(float deltaTime)
        {
            if (_gc.Weather.RainIntensity > 0.05f)
            {
                IsWet = _wetnessValue >= 1f;

                var wetRate = _gc.Weather.RainIntensity * RainWetnessGainRate;

                // Check for clothes water resistance
                var waterResistance = _gc.Body.Clothes.Sum(x => x.WaterResistance);
                var clothesGroup = ClothesGroups.Instance.GetCompleteClothesGroup(_gc);

                if (clothesGroup != null)
                    waterResistance += clothesGroup.WaterResistanceBonus;

                wetRate *= (1 - waterResistance / 100f);

                _wetnessValue += wetRate * deltaTime;

                ////("Wet rate " + wetRate * deltaTime + "% per second");
                
                if (_wetnessValue > 100f)
                    _wetnessValue = 100f;

                _lastGettingWetTime = _gc.WorldTime;

                return;
            }
        }

        private bool CheckForUnderwater()
        {
            if (_gc.Player.IsUnderWater || _gc.Player.IsSwimming)
            {
                IsWet = true;
                
                _wetnessValue = 100f;
                _lastGettingWetTime = _gc.WorldTime;

                return true;
            }

            return false;
        }

        private void CheckForDrying(float deltaTime)
        {
            if (_gc.Weather.RainIntensity > 0.001f)
                return;

            // No sense to dry under water
            if (_gc.Player.IsUnderWater || _gc.Player.IsSwimming)
                return;

            // Drying
            var currentRate = NormalDryRate;

            if (_gc.Weather.Temperature <= FreezeTemperature)
                currentRate = FreezeDryRate;
            else if (_gc.Weather.Temperature <= ColdTemperature)
                currentRate = ColdDryRate;
            else if (_gc.Weather.Temperature >= HotTemperature)
                currentRate = HotDryRate;

            var windPerc = _gc.Weather.WindSpeed / WindSpeedForMaxDrying;
            var windBonus = Helpers.Lerp(0f, MaxWindDryingRate, windPerc > 1f ? 1f : windPerc); 
            
            var dryingRate = currentRate + windBonus;

            _wetnessValue -= dryingRate * deltaTime;

            // ("Drying rate " + dryingRate * deltaTime + "% per second");
            
            IsWet = _wetnessValue >= 1f;

            if (_wetnessValue <= 0f)
            {
                _wetnessValue = 0f;
                _lastGettingWetTime = null;
            }
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new WetnessControllerSnippet
            {
                IsWet = this.IsWet,
                WetnessValue = _wetnessValue,
                LastGettingWetTime = _lastGettingWetTime
            };
               
            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (WetnessControllerSnippet)savedState;

            IsWet = state.IsWet;

            _wetnessValue = state.WetnessValue;
            _lastGettingWetTime = state.LastGettingWetTime;
        }

        #endregion

    }
}
