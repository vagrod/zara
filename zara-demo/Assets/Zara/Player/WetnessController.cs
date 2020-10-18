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

        public const float HotDryRate    = 0.05f;   // percent per real second
        public const float NormalDryRate = 0.025f;  // percent per real second
        public const float ColdDryRate   = 0.01f;   // percent per real second
        public const float FreezeDryRate = 0.005f;  // percent per real second

        public const float RainWetnessGainRate = 80.7f; // max percent per real second

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

            CheckForDrying();
        }

        private void CheckForRain(float deltaTime)
        {
            if (_gc.Weather.RainIntensity > 0.05f)
            {
                IsWet = _wetnessValue >= 1f;

                var wetRate = _gc.Weather.RainIntensity * deltaTime * RainWetnessGainRate;

                // Check for clothes water resistance
                var waterResistance = _gc.Body.Clothes.Sum(x => x.WaterResistance);
                var clothesGroup = ClothesGroups.Instance.GetCompleteClothesGroup();

                if (clothesGroup != null)
                    waterResistance += clothesGroup.WaterResistanceBonus;

                wetRate *= (1 - waterResistance / 100f);

                ////("Wet rate " + wetRate + "% per second");

                _wetnessValue += wetRate;

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

        private void CheckForDrying()
        {
            if (_gc.Weather.RainIntensity > 0.001f)
                return;

            // No sense to dry under water
            if (_gc.Player.IsUnderWater || _gc.Player.IsSwimming)
                return;

            // Drying
            var currentRate = 0f;

            if (_gc.Weather.Temperature <= FreezeTemperature)
                currentRate = FreezeDryRate;
            else if (_gc.Weather.Temperature <= ColdTemperature)
                currentRate = ColdDryRate;
            else if (_gc.Weather.Temperature <= NormalTemperature)
                currentRate = NormalDryRate;
            else if (_gc.Weather.Temperature >= HotTemperature)
                currentRate = HotDryRate;

            var heatBonus = 0f;
            var dryingRate = currentRate + heatBonus;

            ////("Drying rate " + dryingRate + "% per second");

            _wetnessValue -= dryingRate;

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
