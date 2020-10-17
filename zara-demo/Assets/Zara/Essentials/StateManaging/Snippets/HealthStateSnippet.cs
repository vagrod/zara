using System;
using System.Collections.Generic;
using ZaraEngine;
using ZaraEngine.Diseases;

namespace ZaraEngine.StateManaging
{

    public class HealthStateSnippet : SnippetBase
    {

        public HealthStateSnippet() : base() { }
        public HealthStateSnippet(object contract) : base(contract) { }

        #region Data Fields

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

        #endregion 

        public override object ToContract()
        {
            return new HealthStateStateContract
            {
                BloodPressureTop = this.BloodPressureTop,
                BloodPressureBottom = this.BloodPressureBottom,
                HeartRate = this.HeartRate,
                BloodPercentage = this.BloodPercentage,
                FoodPercentage = this.FoodPercentage,
                WaterPercentage = this.WaterPercentage,
                OxygenPercentage = this.OxygenPercentage,
                StaminaPercentage = this.StaminaPercentage,
                FatiguePercentage = this.FatiguePercentage,
                BodyTemperature = this.BodyTemperature,
                LastSleepTime = this.LastSleepTime,
                CheckTime = this.CheckTime,
                IsBloodLoss = this.IsBloodLoss,
                IsActiveInjury = this.IsActiveInjury,
                IsActiveDisease = this.IsActiveDisease,
                IsFoodDisgust = this.IsFoodDisgust,
                IsSleepDisorder = this.IsSleepDisorder,
                CannotRun = this.CannotRun,
                IsLegFracture = this.IsLegFracture,
                ActiveDiseasesWorstLevel = (int)this.ActiveDiseasesWorstLevel
            };
        }

        public override void FromContract(object o)
        {
            var c = (HealthStateStateContract)o;

            BloodPressureTop = c.BloodPressureTop;
            BloodPressureBottom = c.BloodPressureBottom;
            HeartRate = c.HeartRate;
            BloodPercentage = c.BloodPercentage;
            FoodPercentage = c.FoodPercentage;
            WaterPercentage = c.WaterPercentage;
            OxygenPercentage = c.OxygenPercentage;
            StaminaPercentage = c.StaminaPercentage;
            FatiguePercentage = c.FatiguePercentage;
            BodyTemperature = c.BodyTemperature;
            LastSleepTime = c.LastSleepTime;
            CheckTime = c.CheckTime;
            IsBloodLoss = c.IsBloodLoss;
            IsActiveInjury = c.IsActiveInjury;
            IsActiveDisease = c.IsActiveDisease;
            IsFoodDisgust = c.IsFoodDisgust;
            IsSleepDisorder = c.IsSleepDisorder;
            CannotRun = c.CannotRun;
            IsLegFracture = c.IsLegFracture;
            ActiveDiseasesWorstLevel = (DiseaseLevels)c.ActiveDiseasesWorstLevel;

            ChildStates.Clear();
        }

    }

}