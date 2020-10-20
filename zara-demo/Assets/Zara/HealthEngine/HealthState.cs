using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries;
using ZaraEngine.StateManaging;

namespace ZaraEngine.HealthEngine
{
    public class HealthState : IAcceptsStateChange
    {

        private readonly IGameController _gc;

        public HealthState(IGameController gc)
        {
            _gc = gc;

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

        public DiseaseDefinitionBase WorstDisease { get; set; }

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
            return new HealthState(_gc)
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
                OxygenPercentage = this.OxygenPercentage,
                WorstDisease = this.WorstDisease
            };
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new HealthStateSnippet
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
                ActiveDiseasesWorstLevel = this.ActiveDiseasesWorstLevel,
                WorstDiseaseId = this.WorstDisease?.Id
            };

            var diseases = ActiveDiseases.ConvertAll(x => (ActiveDiseaseSnippet)x.GetState());
            var injuries = ActiveInjuries.ConvertAll(x => (ActiveInjurySnippet)x.GetState());

            state.ChildStates.Add("ActiveDiseasesAndInjuries", new ActiveDiseasesAndInjuriesSnippet { 
                ActiveDiseases = diseases,
                ActiveInjuries = injuries
            });

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (HealthStateSnippet)savedState;

            BloodPressureTop = state.BloodPressureTop;
            BloodPressureBottom = state.BloodPressureBottom;
            HeartRate = state.HeartRate;
            BloodPercentage = state.BloodPercentage;
            FoodPercentage = state.FoodPercentage;
            WaterPercentage = state.WaterPercentage;
            OxygenPercentage = state.OxygenPercentage;
            StaminaPercentage = state.StaminaPercentage;
            FatiguePercentage = state.FatiguePercentage;
            BodyTemperature = state.BodyTemperature;
            LastSleepTime = state.LastSleepTime;
            CheckTime = state.CheckTime;
            IsBloodLoss = state.IsBloodLoss;
            IsActiveInjury = state.IsActiveInjury;
            IsActiveDisease = state.IsActiveDisease;
            IsFoodDisgust = state.IsFoodDisgust;
            IsSleepDisorder = state.IsSleepDisorder;
            CannotRun = state.CannotRun;
            IsLegFracture = state.IsLegFracture;
            ActiveDiseasesWorstLevel = state.ActiveDiseasesWorstLevel;

            var diseasesAndInjuriesData = (ActiveDiseasesAndInjuriesSnippet)state.ChildStates["ActiveDiseasesAndInjuries"];

            CreateAndLinkActiveDiseasesAndInjuries(state.WorstDiseaseId, diseasesAndInjuriesData.ActiveDiseases, diseasesAndInjuriesData.ActiveInjuries);
        }

        private void CreateAndLinkActiveDiseasesAndInjuries(Guid? worstDiseaseId, List<ActiveDiseaseSnippet> diseasesData, List<ActiveInjurySnippet> injuriesData)
        {
            ActiveInjuries.Clear();
            ActiveDiseases.Clear();

            var injuriesMapping = new Dictionary<Guid, Guid>(); // old id, new id

            foreach (var injData in injuriesData)
            {
                var inj = new ActiveInjury(_gc);

                inj.RestoreState(injData);

                injuriesMapping.Add(injData.InjuryId, inj.Injury.Id);

                ActiveInjuries.Add(inj);
            }

            var diseasesMapping = new Dictionary<Guid, Guid>(); // old id, new id

            foreach (var diseaseData in diseasesData)
            {
                var injKey = diseaseData.InjuryId.HasValue ? (injuriesMapping.ContainsKey(diseaseData.InjuryId.Value) ? injuriesMapping[diseaseData.InjuryId.Value] : (Guid?)null) : (Guid?)null;
                var linkedInj = injKey.HasValue ? ActiveInjuries.FirstOrDefault(x => x.Injury.Id == injKey.Value) : null;
                var disease = new ActiveDisease(_gc, linkedInj);

                disease.RestoreState(diseaseData);

                diseasesMapping.Add(diseaseData.DiseaseId, disease.Disease.Id);

                ActiveDiseases.Add(disease);
            }

            var diseaseKey = worstDiseaseId.HasValue ? (diseasesMapping.ContainsKey(worstDiseaseId.Value) ? diseasesMapping[worstDiseaseId.Value] : (Guid?)null) : (Guid?)null;
            var worstDisease = diseaseKey.HasValue ? ActiveDiseases.FirstOrDefault(x => x.Disease.Id == diseaseKey.Value) : null;

            WorstDisease = worstDisease?.Disease;

            injuriesMapping.Clear();
            diseasesMapping.Clear();
        }

        #endregion

    }
}
