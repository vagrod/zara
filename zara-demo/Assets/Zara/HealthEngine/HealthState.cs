using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries;
using UnityEngine;

namespace ZaraEngine.HealthEngine
{
    [Serializable]
    public class HealthState
    {

        public HealthState()
        {
            ActiveDiseases = new List<ActiveDisease>();
            ActiveInjuries = new List<ActiveInjury>();
        }

        [SerializeField]
        public float BloodPressureTop { get; set; }

        [SerializeField]
        public float BloodPressureBottom { get; set; }

        [SerializeField]
        public float HeartRate { get; set; }

        [SerializeField]
        public float BloodPercentage { get; set; }

        [SerializeField]
        public float FoodPercentage { get; set; }

        [SerializeField]
        public float WaterPercentage { get; set; }

        [SerializeField]
        public float StaminaPercentage { get; set; }

        [SerializeField]
        public float FatiguePercentage { get; set; }

        [SerializeField]
        public float BodyTemperature { get; set; }

        [SerializeField]
        public List<ActiveDisease> ActiveDiseases { get; set; }

        [SerializeField]
        public List<ActiveInjury> ActiveInjuries { get; set; }

        [SerializeField]
        public DateTime LastSleepTime { get; set; }

        [SerializeField]
        public DateTime CheckTime { get; set; }

        [SerializeField]
        public bool IsBloodLoss { get; set; }

        [SerializeField]
        public bool IsActiveInjury { get; set; }

        [SerializeField]
        public bool IsActiveDisease { get; set; }

        [SerializeField]
        public bool IsFoodDisgust { get; set; }

        [SerializeField]
        public bool IsSleepDisorder { get; set; }

        [SerializeField]
        public bool CannotRun { get; set; }

        [SerializeField]
        public bool IsLegFracture { get; set; }

        [SerializeField]
        public DiseaseLevels ActiveDiseasesWorstLevel { get; set; }

        public static HealthState Empty { get { return default(HealthState); } }

        private const float Tolerance = 0.00001f;

        public bool IsEmpty
        {
            get
            {
                return Math.Abs(BloodPressureTop) < Tolerance && Math.Abs(BloodPressureBottom) < Tolerance && Math.Abs(HeartRate) < Tolerance &&
                       Math.Abs(BloodPercentage) < Tolerance && Math.Abs(FoodPercentage) < Tolerance && Math.Abs(WaterPercentage) < Tolerance &&
                       Math.Abs(StaminaPercentage) < Tolerance;
            }
        }

        public void SetStaminaLevel(float value)
        {
            if (value > 100f)
            {
                StaminaPercentage = 100f;
                return;
            }

            if (Math.Abs(value) < Tolerance)
            {
                StaminaPercentage = 0f;
                return;
            }

            StaminaPercentage = value;
        }

        public void SetFoodLevel(float value)
        {
            if (value > 100f)
            {
                FoodPercentage = 100f;
                return;
            }

            if (Math.Abs(value) < Tolerance)
            {
                FoodPercentage = 0f;
                return;
            }

            FoodPercentage = value;
        }

        public void SetWaterLevel(float value)
        {
            if (value > 100f)
            {
                WaterPercentage = 100f;
                return;
            }

            if (Math.Abs(value) < Tolerance)
            {
                WaterPercentage = 0f;
                return;
            }

            WaterPercentage = value;
        }

        public void SetBloodLevel(float value)
        {
            if (value > 100f)
            {
                BloodPercentage = 100f;
                return;
            }

            if (Math.Abs(value) < Tolerance)
            {
                BloodPercentage = 0f;
                return;
            }

            BloodPercentage = value;
        }

        public object GetActive<T>()
            where T : class
        {
            if (typeof(T).IsSubclassOf(typeof(DiseaseDefinitionBase)))
            {
                return ActiveDiseases.ToList().FirstOrDefault(x => x.IsActiveNow && x.Disease.GetType() == typeof(T));
            }

            if (typeof(T).IsSubclassOf(typeof(InjuryBase)))
            {
                return ActiveInjuries.ToList().FirstOrDefault(x => x.IsActiveNow && x.Injury.GetType() == typeof(T));
            }

            return null;
        }

        public object GetActiveOrScheduled<T>(DateTime currentTime)
            where T : class
        {
            if (typeof(T).IsSubclassOf(typeof(DiseaseDefinitionBase)))
            {
                return ActiveDiseases.ToList().FirstOrDefault(x => (x.IsActiveNow || x.DiseaseStartTime >= currentTime) && x.Disease.GetType() == typeof(T));
            }

            if (typeof(T).IsSubclassOf(typeof(InjuryBase)))
            {
                return ActiveInjuries.ToList().FirstOrDefault(x => (x.IsActiveNow || x.InjuryTriggerTime >= currentTime) && x.Injury.GetType() == typeof(T));
            }

            return null;
        }

        public bool HasActive<T>()
            where T: class
        {
            return GetActive<T>() != null;
        }

        public bool HasActiveOrScheduled<T>(DateTime currentTime)
            where T : class
        {
            return GetActiveOrScheduled<T>(currentTime) != null;
        }

        public HealthState Clone(DateTime? currentTime)
        {
            return new HealthState
            {
                ActiveDiseases = currentTime == null ? this.ActiveDiseases.ToList() : this.ActiveDiseases.Where(x => x.IsActiveNow || x.DiseaseStartTime >= currentTime || Math.Abs((x.DiseaseStartTime - currentTime.Value).TotalMinutes) < 10).ToList(),
                StaminaPercentage = this.StaminaPercentage,
                WaterPercentage = this.WaterPercentage,
                FoodPercentage = this.FoodPercentage,
                HeartRate = this.HeartRate,
                BodyTemperature = this.BodyTemperature,
                BloodPercentage = this.BloodPercentage,
                BloodPressureTop = this.BloodPressureTop,
                BloodPressureBottom = this.BloodPressureBottom,
                ActiveInjuries = currentTime == null ? this.ActiveInjuries.ToList() : this.ActiveInjuries.Where(x => x.IsActiveNow || x.InjuryTriggerTime >= currentTime || Math.Abs((x.InjuryTriggerTime - currentTime.Value).TotalMinutes) < 10).ToList(),
                FatiguePercentage = this.FatiguePercentage,
                LastSleepTime = this.LastSleepTime,
                CheckTime = this.CheckTime
            };
        }

    }
}
