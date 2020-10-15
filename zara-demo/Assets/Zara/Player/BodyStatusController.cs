using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ZaraEngine.HealthEngine;
using ZaraEngine.Inventory;
using Foundation.Databinding;

namespace ZaraEngine.Player
{
    public class BodyStatusController
    {

        private const float WarmthLevelUpdateInterval = 1.6f; // Real reconds
        private const float WetnessLevelUpdateInterval = 1f;  // Real reconds

        private const int SleepHealthChecksCount = 5; // times

        private readonly IGameController _gc;
        private readonly WetnessController _wetnessController;

        private float _warmthLevelTimeoutCounter;
        private float _wetnessLevelTimeoutCounter;

        private float _warmthLerpTarget;
        private float? _warmthLerpCounter;
        private float _warmthLerpBase;

        public BodyStatusController(IGameController gc)
        {
            _gc = gc;

            _wetnessController = new WetnessController(gc);

            Appliances = new List<MedicalBodyAppliance>();
            Clothes = new ObservableCollection<ClothesItemBase>();
        }

        public void Initialize()
        {
            _wetnessController.Initialize();
        }

        public float WetnessLevel
        {
            get { return _wetnessController.WetnessLevel; }
        }

        public bool IsWet
        {
            get { return _wetnessController.IsWet; }
        }

        public List<MedicalBodyAppliance> Appliances { get; private set; }

        public ObservableCollection<ClothesItemBase> Clothes { get; private set; }

        public float WarmthLevelCached { get; private set; }

        public bool IsSleeping { get; private set; }

        private Action _wakeUpAction;
        private float _sleepingCounter; // real seconds
        private float _sleepDurationGameHours; // game hours
        private Action<DateTime> _sleepTimeAdvanceFunc;
        private float _sleepHealthCheckPeriod; // real seconds
        private int _sleepHealthChecksLeft;
        private DateTime _sleepStartTime;
        private float _fatigueValueAfterSleep;

        public bool Sleep(float hours, float realDurationSeconds, Action onWakeUp, Action<DateTime> timeAdvanceFunc){
            if(!_gc.WorldTime.HasValue)
                return false;

            if(IsSleeping)
                return true;

            //if(!_gc.Health.IsSleepAllowed)
            //    return false;

            if(_gc.Health.UnconsciousMode)
                return false;

            IsSleeping = true;

            _gc.Health.Status.LastSleepTime = _gc.WorldTime.Value;

            var totalSleepSeconds = hours * 60f * 60f;

            _sleepStartTime = _gc.WorldTime.Value;
            _wakeUpAction = onWakeUp;
            _sleepTimeAdvanceFunc = timeAdvanceFunc;
            _sleepDurationGameHours = hours;
            _sleepHealthChecksLeft = SleepHealthChecksCount;
            _sleepHealthCheckPeriod = realDurationSeconds / SleepHealthChecksCount;
            _fatigueValueAfterSleep = 0f;

            // Check for diseases and prolong healthy states if any
            var activeDiseases = _gc.Health.Status.ActiveDiseases.ToList().Where(x => x.IsActiveNow);

            foreach (var disease in activeDiseases)
            {
                var stage = disease.GetActiveStage(_gc.WorldTime.Value);

                if (stage != null)
                {
                    if (stage.Level == ZaraEngine.Diseases.DiseaseLevels.HealthyStage)
                    {
                        stage.StageDuration += TimeSpan.FromSeconds(totalSleepSeconds);
                        stage.WillEndAt = stage.WillEndAt.Value.AddSeconds(totalSleepSeconds);
                    }
                }
            }

            _gc.Health.Status.OxygenPercentage = 100f;
            _gc.Health.UnconsciousMode = true;

            return true;
        }

        public void UpdateWarmthLevelCache()
        {
            _warmthLerpCounter = null;
            _warmthLerpBase = WarmthLevelCached;
            _warmthLerpTarget = GetWarmthLevel();
            _warmthLerpCounter = 0f;
        }

        public void UpdateWarmthLevelCacheImmediately()
        {
            _warmthLerpCounter = null;

            WarmthLevelCached = GetWarmthLevel();

            _warmthLerpBase = WarmthLevelCached;
            _warmthLerpTarget = WarmthLevelCached;
            _warmthLerpCounter = WarmthLevelUpdateInterval;
        }

        /// <summary>
        /// Values between -5..+5 is in a comfort zone.
        /// </summary>
        public float GetWarmthLevel()
        {
            const float comfortTemperatureNaked           = 22f; // Degrees in C
            const float maximumWetnessTemperatureDecrease = 10f; // Degrees in C
            const float maximumWindTemperatureDecrease    = 15f; // Degrees in C

            var temp = _gc.Weather.Temperature;

            var wetnessTemperatureBonus = -(WetnessLevel / 100f) * maximumWetnessTemperatureDecrease;

            var windSpeed = _gc.Weather.WindSpeed;
            var windColdness = (windSpeed * (temp / 35f) - windSpeed) / 35f; // -1..+1 scale
            var windTemperatureBonus = windColdness * maximumWindTemperatureDecrease;

            if (windTemperatureBonus > 0f)
                windTemperatureBonus = 0; // only cold wind counts

            var finalTemp = temp + wetnessTemperatureBonus + windTemperatureBonus;

            var coldResistance = Clothes.Sum(x => x.ColdResistance);
            var clothesGroup = ClothesGroups.Instance.GetCompleteClothesGroup();

            if (clothesGroup != null)
                coldResistance += clothesGroup.ColdResistanceBonus;

            return (finalTemp * (1f - coldResistance / 100f)) - (comfortTemperatureNaked - coldResistance / 2f) + finalTemp * (coldResistance / 100f);
        }

        public void Check(float deltaTime)
        {

            #region Sleeping

            if(IsSleeping){
                _sleepingCounter += deltaTime;

                if(_sleepingCounter >= _sleepHealthCheckPeriod){
                    // need to check health and advance time

                    //($"Sleep iteration done. Checks was {_sleepHealthChecksLeft}, now it's {_sleepHealthChecksLeft-1}");

                    _sleepingCounter = 0f;
                    _sleepHealthChecksLeft--;

                    if(_sleepHealthChecksLeft == 0){
                        // Done sleeping

                        var newWorldTime = _sleepStartTime.AddHours(_sleepDurationGameHours);

                        //($"Done sleeping. Wake up exact time is {newWorldTime.ToShortTimeString()}, warmth deltaTime is {deltaTime}");

                        _sleepTimeAdvanceFunc?.Invoke(newWorldTime);

                        _gc.Health.UnconsciousMode = false;
                        _gc.Health.SetFatiguePercentage(_fatigueValueAfterSleep);
                        _gc.Health.Status.CheckTime = newWorldTime;

                        IsSleeping = false;

                        _wakeUpAction?.Invoke();
                    } else {
                        var iteration = SleepHealthChecksCount - _sleepHealthChecksLeft;
                        var newWorldTime = _sleepStartTime.AddHours((_sleepDurationGameHours / SleepHealthChecksCount) * iteration);

                        deltaTime = (float)(newWorldTime - _gc.WorldTime.Value).TotalSeconds;

                        //($"Mid-sleep check. New mid-sleep time is {newWorldTime.ToShortTimeString()}, warmth deltaTime is {deltaTime}");

                        _sleepTimeAdvanceFunc?.Invoke(newWorldTime);

                        _gc.Health.Status.LastSleepTime = newWorldTime;
                    }
                }
            }

            #endregion 

            #region Lerping Warmth Level

            if (_warmthLerpCounter.HasValue)
            {

                if (_warmthLerpCounter < WarmthLevelUpdateInterval)
                    _warmthLerpCounter += deltaTime;

                if(_warmthLerpCounter > WarmthLevelUpdateInterval)
                    _warmthLerpCounter = WarmthLevelUpdateInterval;

                WarmthLevelCached = Helpers.Lerp(_warmthLerpBase, _warmthLerpTarget, _warmthLerpCounter.Value / WarmthLevelUpdateInterval);
            }

            #endregion

            #region Warmth Level Refresh

            _warmthLevelTimeoutCounter += deltaTime;

            if(_warmthLevelTimeoutCounter > WarmthLevelUpdateInterval)
                _warmthLevelTimeoutCounter = WarmthLevelUpdateInterval;

            if (_warmthLevelTimeoutCounter >= WarmthLevelUpdateInterval)
            {
                _warmthLerpCounter = null;

                _warmthLevelTimeoutCounter = 0f;

                UpdateWarmthLevelCache();

                _warmthLerpCounter = 0;
            }

            #endregion

            #region Wetness Level Refresh

            _wetnessLevelTimeoutCounter += deltaTime;
            
            if(_wetnessLevelTimeoutCounter > WetnessLevelUpdateInterval)
                _wetnessLevelTimeoutCounter = WetnessLevelUpdateInterval;

            if (_wetnessLevelTimeoutCounter >= WetnessLevelUpdateInterval)
            {
                _wetnessLevelTimeoutCounter = 0f;

                _wetnessController.Check(deltaTime);
            }

            #endregion

        }

    }
}
