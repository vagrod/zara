using System;
using System.Linq;
using System.Text;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.HealthEngine.SideEffects
{
    public class ClothesSideEffectsController : IAcceptsStateChange
    {

        private const int LightClothesWeight  = 2000; // grams
        private const int MediumClothesWeight = 5000; // grams
        private const int HeavyClothesWeight  = 7000; // grams

        private const float HeavyClothesSpeedDecrease  = -1f;
        private const float MediumClothesSpeedDecrease = -0.5f;
        private const float LightClothesSpeedDecrease  = -0.2f;

        private const float HeavyClothesStaminaBonus  = 0.0055f;
        private const float MediumClothesStaminaBonus = 0.001f;
        private const float LightClothesStaminaBonus  = 0.0005f;

        private const float MaxHeartRateIncrease       = 35f;  // bpm
        private const float MaxBodyTemperatureIncrease = 1f;   // С degrees

        private const int ClothesEffectPeakDuration = 15; // Game minutes
        private const int AutoReLerpInterval = 60;        // Game minutes

        private DateTime? _lastClothesChangeTime; // Game time
        private DateTime? _lastAutoReLerpTime;    // Game time

        private float _targetBodyTemperatureDelta;
        private float _targetHeartRateDelta;
        private float _currentTemperatureBonus;
        private float _currentHeartRateBonus;

        private const int ExtraHeatValue = 5;

        private readonly IGameController _gc;

        public float PlayerRunSpeedBonus { get; private set; }

        public float HeartRateBonus { get; private set; }
        public float BodyTemperatureBonus { get; private set; }
        public float StaminaBonus { get; private set; }

        public ClothesSideEffectsController(IGameController gc)
        {
            _gc = gc;

            // TODO: Call ReLerp on Environment change
        }

        public void Initialize()
        {
            _gc.Body.Clothes.OnAdd += OnClothesAdd;
            _gc.Body.Clothes.OnRemove += OnClothesRemove;
        }

        public void Check()
        {
            if (!_gc.WorldTime.HasValue)
                return;

            var clothesWeight = _gc.Body.Clothes.Sum(x => x.WeightGrammsPerUnit);

            if (clothesWeight >= HeavyClothesWeight)
            {
                PlayerRunSpeedBonus = HeavyClothesSpeedDecrease;
                StaminaBonus = HeavyClothesStaminaBonus;
            }
            else if (clothesWeight >= MediumClothesWeight)
            {
                PlayerRunSpeedBonus = MediumClothesSpeedDecrease;
                StaminaBonus = MediumClothesStaminaBonus;
            }
            else if (clothesWeight > LightClothesWeight)
            {
                PlayerRunSpeedBonus = LightClothesSpeedDecrease;
                StaminaBonus = LightClothesStaminaBonus;
            }

            if (!_lastAutoReLerpTime.HasValue)
                _lastAutoReLerpTime = _gc.WorldTime.Value;

            var currentLerp = 1f;

            if (_lastClothesChangeTime.HasValue)
            {
                var secondsSinceClothesChange = (float) (_gc.WorldTime.Value - _lastClothesChangeTime.Value).TotalSeconds;
                var secondsUntilPeakValue = ClothesEffectPeakDuration * 60f;

                currentLerp = Helpers.Clamp01(secondsSinceClothesChange / secondsUntilPeakValue);
            }

            if ((_gc.WorldTime.Value - _lastAutoReLerpTime.Value).TotalSeconds > AutoReLerpInterval * 60f && Math.Abs(currentLerp - 1f) < 0.000001)
            {
                // If clothes-change-related lerping is done

                _lastAutoReLerpTime = _gc.WorldTime.Value;

                ReLerp();

                return;
            }

            BodyTemperatureBonus = Helpers.Lerp(_currentTemperatureBonus, _targetBodyTemperatureDelta, currentLerp);
            HeartRateBonus = Helpers.Lerp(_currentHeartRateBonus, _targetHeartRateDelta, currentLerp);
        }

        private void OnClothesAdd(ClothesItemBase clothes)
        {
            if (!_gc.WorldTime.HasValue)
                return;

            ReLerp();
        }

        private void OnClothesRemove(ClothesItemBase clothes)
        {
            if (!_gc.WorldTime.HasValue)
                return;

            ReLerp();
        }

        private void ReLerp()
        {
            _currentTemperatureBonus = BodyTemperatureBonus;
            _currentHeartRateBonus = HeartRateBonus;

            _lastClothesChangeTime = _gc.WorldTime.Value;

            var warmthLevel = _gc.Body.GetWarmthLevel();

            if (warmthLevel > ExtraHeatValue) 
            {
                var impactDelta = (warmthLevel - ExtraHeatValue) / 20f;

                _targetBodyTemperatureDelta = Helpers.Clamp(MaxBodyTemperatureIncrease * impactDelta, 0f, MaxBodyTemperatureIncrease);
                _targetHeartRateDelta = Helpers.Clamp(MaxHeartRateIncrease * impactDelta, 0f, MaxHeartRateIncrease);

                return;
            }

            // No clothes effects: lerping back to normal
            _targetBodyTemperatureDelta = 0f;
            _targetHeartRateDelta = 0f;
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new ClothesHealthEffectsSnippet
            {
                BodyTemperatureBonus = this.BodyTemperatureBonus,
                HeartRateBonus = this.HeartRateBonus,
                PlayerRunSpeedBonus = this.PlayerRunSpeedBonus,
                StaminaBonus = this.StaminaBonus,
                CurrentHeartRateBonus = _currentHeartRateBonus,
                CurrentTemperatureBonus = _currentTemperatureBonus,
                LastAutoReLerpTime = _lastAutoReLerpTime,
                LastClothesChangeTime = _lastClothesChangeTime,
                TargetBodyTemperatureDelta = _targetBodyTemperatureDelta,
                TargetHeartRateDelta = _targetHeartRateDelta
            };

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (ClothesHealthEffectsSnippet)savedState;

            BodyTemperatureBonus = state.BodyTemperatureBonus;
            HeartRateBonus = state.HeartRateBonus;
            PlayerRunSpeedBonus = state.PlayerRunSpeedBonus;
            StaminaBonus = state.StaminaBonus;

            _currentHeartRateBonus = state.CurrentHeartRateBonus;
            _currentTemperatureBonus = state.CurrentTemperatureBonus;
            _lastAutoReLerpTime = state.LastAutoReLerpTime;
            _lastClothesChangeTime = state.LastClothesChangeTime;
            _targetBodyTemperatureDelta = state.TargetBodyTemperatureDelta;
            _targetHeartRateDelta = state.TargetHeartRateDelta;
        }

        #endregion 

    }
}
