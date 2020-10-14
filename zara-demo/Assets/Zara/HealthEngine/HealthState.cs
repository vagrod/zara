using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries;

namespace ZaraEngine.HealthEngine
{
    public class HealthState
    {

        public HealthState()
        {
            ActiveDiseases = new List<ActiveDisease>();
            ActiveInjuries = new List<ActiveInjury>();
        }

        public float BloodPressureTop { get; set; }

        public float BloodPressureBottom { get; set; }

        public float HeartRate { get; set; }

        public float BloodPercentage { get; set; }

        public float FoodPercentage { get; set; }

        public float WaterPercentage { get; set; }

        public float OxygenPercentage { get; set; }

        public float StaminaPercentage { get; set; }

        public float FatiguePercentage { get; set; }

        public float BodyTemperature { get; set; }

        public List<ActiveDisease> ActiveDiseases { get; set; }

        public List<ActiveInjury> ActiveInjuries { get; set; }

        public DateTime LastSleepTime { get; set; }

        public DateTime CheckTime { get; set; }

        public bool IsBloodLoss { get; set; }

        public bool IsActiveInjury { get; set; }

        public bool IsActiveDisease { get; set; }

        public bool IsFoodDisgust { get; set; }

        public bool IsSleepDisorder { get; set; }

        public bool CannotRun { get; set; }

        public bool IsLegFracture { get; set; }

        public DiseaseLevels ActiveDiseasesWorstLevel { get; set; }

        public static HealthState Empty { get { return default(HealthState); } }

        private const float Tolerance = 0.00001f;

        public bool IsEmpty
        {
            get
            {
                return Math.Abs(BloodPressureTop) < Tolerance && Math.Abs(BloodPressureBottom) < Tolerance && Math.Abs(HeartRate) < Tolerance &&
                       Math.Abs(BloodPercentage) < Tolerance && Math.Abs(FoodPercentage) < Tolerance && Math.Abs(WaterPercentage) < Tolerance &&
                       Math.Abs(StaminaPercentage) < Tolerance && Math.Abs(OxygenPercentage) < Tolerance;
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

        public void SetOxygenLevel(float value)
        {
            if (value > 100f)
            {
                OxygenPercentage = 100f;
                return;
            }

            if (Math.Abs(value) < Tolerance)
            {
                OxygenPercentage = 0f;
                return;
            }

            OxygenPercentage = value;
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

        public List<ActiveDisease> GetActualDiseases(DateTime? currentTime)
        {
            return currentTime == null ? this.ActiveDiseases.ToList() : this.ActiveDiseases.Where(x => x.IsActiveNow || x.DiseaseStartTime >= currentTime || Math.Abs((x.DiseaseStartTime - currentTime.Value).TotalMinutes) < 10).ToList();
        }

        public List<ActiveInjury> GetActualInjuries(DateTime? currentTime)
        {
            return currentTime == null ? this.ActiveInjuries.ToList() : this.ActiveInjuries.Where(x => x.IsActiveNow || x.InjuryTriggerTime >= currentTime || Math.Abs((x.InjuryTriggerTime - currentTime.Value).TotalMinutes) < 10).ToList();
        }

        public HealthState Clone(DateTime? currentTime)
        {
            return new HealthState
            {
                ActiveDiseases = GetActualDiseases(currentTime),
                StaminaPercentage = this.StaminaPercentage,
                WaterPercentage = this.WaterPercentage,
                FoodPercentage = this.FoodPercentage,
                HeartRate = this.HeartRate,
                BodyTemperature = this.BodyTemperature,
                BloodPercentage = this.BloodPercentage,
                BloodPressureTop = this.BloodPressureTop,
                BloodPressureBottom = this.BloodPressureBottom,
                ActiveInjuries = GetActualInjuries(currentTime),
                FatiguePercentage = this.FatiguePercentage,
                LastSleepTime = this.LastSleepTime,
                CheckTime = this.CheckTime,
                OxygenPercentage = this.OxygenPercentage
            };
        }

    }
}
