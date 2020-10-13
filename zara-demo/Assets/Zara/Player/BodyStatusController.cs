using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ZaraEngine.HealthEngine;
using ZaraEngine.Inventory;
using Foundation.Databinding;
using UnityEngine;

namespace ZaraEngine.Player
{
    public class BodyStatusController
    {

        private const float WarmthLevelUpdateInterval = 1.6f; // Real reconds
        private const float WetnessLevelUpdateInterval = 1f;  // Real reconds

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

        public void Check()
        {

            #region Lerping Warmth Level

            if (_warmthLerpCounter.HasValue)
            {

                if (_warmthLerpCounter < WarmthLevelUpdateInterval)
                    _warmthLerpCounter += Time.deltaTime;

                WarmthLevelCached = Mathf.Lerp(_warmthLerpBase, _warmthLerpTarget, _warmthLerpCounter.Value / WarmthLevelUpdateInterval);
            }

            #endregion

            #region Warmth Level Refresh

            _warmthLevelTimeoutCounter += Time.deltaTime;

            if (_warmthLevelTimeoutCounter > WarmthLevelUpdateInterval)
            {
                _warmthLerpCounter = null;

                _warmthLevelTimeoutCounter = 0f;

                UpdateWarmthLevelCache();

                _warmthLerpCounter = 0;
            }

            #endregion

            #region Wetness Level Refresh

            _wetnessLevelTimeoutCounter += Time.deltaTime;

            if (_wetnessLevelTimeoutCounter > WetnessLevelUpdateInterval)
            {
                _wetnessLevelTimeoutCounter = 0f;

                _wetnessController.Check();
            }

            #endregion

        }

    }
}
