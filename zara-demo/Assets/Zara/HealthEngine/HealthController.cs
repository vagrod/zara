using System;
using System.Collections.Generic;
using System.Linq;
using ZaraEngine.Diseases;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.HealthEngine;
using ZaraEngine.HealthEngine.SideEffects;
using ZaraEngine.Injuries;
using ZaraEngine.Injuries.Stages;
using ZaraEngine.Inventory;
using Foundation.Databinding;
using UnityEditorInternal;

namespace ZaraEngine.HealthEngine {
    [Serializable]
    public class HealthController {

        private readonly IGameController _gc;

        /* ------------------------------------------------------------------------------------------------------------------ *
         * ----------------------------------------------------- Intervals -------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */

        public const float HealthUpdateInterval                      = 1f;                        // Real seconds
        private const float UnconsciousHealthUpdateInterval          = 0.3f;                      // Real seconds
        private const float DiseaseDizzinessCheckInterval            = 9 * HealthUpdateInterval;  // Real seconds
        private const float DiseaseBlackoutsCheckInterval            = 17 * HealthUpdateInterval; // Real seconds
        private const float LowBodyTemperatureDizzinessCheckInterval = 44 * HealthUpdateInterval; // Real seconds
        private const float LowBodyTemperatureBlackoutsCheckInterval = 79 * HealthUpdateInterval; // Real seconds
        private const float DiseaseDeathCheckInterval                = 12 * HealthUpdateInterval; // Real seconds
        private const float VitalsDeathCheckInterval                 = 50 * HealthUpdateInterval; // Real seconds
        private const float SneezeCheckInterval                      = 15 * HealthUpdateInterval; // Real seconds
        private const float CoughCheckInterval                       = 14 * HealthUpdateInterval; // Real seconds
        private const float BloodLevelDizzinessCheckInterval         = 11 * HealthUpdateInterval; // Real seconds
        private const float BloodLevelBlackoutsCheckInterval         = 16 * HealthUpdateInterval; // Real seconds
        private const float BloodLevelDeathCheckInterval             = 21 * HealthUpdateInterval; // Real seconds
        private const float DehydrationDeathCheckInterval            = 25 * HealthUpdateInterval; // Real seconds
        private const float StarvationDeathCheckInterval             = 32 * HealthUpdateInterval; // Real seconds
        private const float OverdoseDeathCheckInterval               = 11 * HealthUpdateInterval; // Real seconds
        private const float HeartFailureDeathCheckInterval           = 24 * HealthUpdateInterval; // Real seconds
        private const float LsdEffectCheckInterval                   = 91 * HealthUpdateInterval; // Real seconds
        private const float FatigueDizzinessCheckInterval            = 10 * HealthUpdateInterval; // Real seconds
        private const float FatigueBlackoutsCheckInterval            = 20 * HealthUpdateInterval; // Real seconds
        private const float FatigueSleepCheckInterval                = 70 * HealthUpdateInterval; // Real seconds
        private const float SedativeSleepCheckInterval               = 30 * HealthUpdateInterval; // Real seconds

        /* ------------------------------------------------------------------------------------------------------------------ *
         * ------------------------------------------------------ Dosage ---------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */

        private const int EpinephrineOverdoseValue   = 2; // Injections
        private const int AntiVenomOverdoseValue     = 4; // Injections
        private const int AtropineOverdoseValue      = 3; // Injections
        private const int MorphineOverdoseValue      = 3; // Injections
        private const int DoripomenOverdoseValue     = 5; // Injections

        private const int AntibioticOverdoseValue    = 6; // Pills
        private const int AspirinOverdoseValue       = 8; // Pills
        private const int AcetaminophenOverdoseValue = 5; // Pills
        private const int LoperamideOverdoseValue    = 9; // Pills
        private const int OseltamivirOverdoseValue   = 8; // Pills
        private const int SedativeOverdoseValue      = 4; // Pills

        private const int SedativeForceSleepValue            = 3; // Pills
        private const int SedativeDosesUntilMaxFatigueImpact = 3; // Pills
        private const int MorphineLsdEffectValue             = 2; // Injections

        /* ------------------------------------------------------------------------------------------------------------------ *
         * ----------------------------------------------------- Chances ---------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */

        private const int BloodLevelBlackoutChance  = 35; // percents
        private const int BloodLevelDizzinessChance = 35; // percents
        private const int BloodLevelDeathChance     = 40; // percents
        private const int DehydrationDeathChance    = 55; // percents
        private const int StarvationDeathChance     = 45; // percents
        private const int VitalsDeathChance         = 9;  // percents
        private const int CoughChanceLight          = 10; // percents
        private const int CoughChanceMedium         = 50; // percents
        private const int OverdoseDeathChance       = 85; // percents
        private const int HeartFailureDeathChance   = 17; // percents
        private const int LsdEffectChance           = 61; // percents
        private const int FatigueBlackoutChance     = 50; // percents
        private const int FatigueDizzinessChance    = 50; // percents
        private const int FatigueSleepChance        = 10; // percents
        private const int SedativeSleepChance       = 21; // percents

        /* ------------------------------------------------------------------------------------------------------------------ *
         * ------------------------------------------------------ Speeds ---------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */

        private const float HealthyWalkSpeed    = 8f;
        private const float HealthyRunSpeed     = 15f;
        private const float HealthyCrouchSpeed  = 5f;
        private const float CriticalWalkSpeed   = 5f;
        private const float CriticalRunSpeed    = 5f;
        private const float CriticalCrouchSpeed = 3f;

        /* ------------------------------------------------------------------------------------------------------------------ *
         * --------------------------------------------------- Fluctuation -------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */

        private const float VitalsFluctuateEquilibriumMargin    = 3f;
        private const int   VitalsFluctuateEveryNChecks         = 2;
        private const float HeartRateFluctuationDelta           = 1f;    // bpm
        private const float BloodPressureTopFluctuationDelta    = 1f;    // mmHg
        private const float BloodPressureBottomFluctuationDelta = 1f;    // mmHg
        private const float BodyTemperatureFluctuationDelta     = 0.07f; // C

        /* ------------------------------------------------------------------------------------------------------------------ *
         * ------------------------------------------------------ Levels ---------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */

        private const float StaminaLevelAffectingPlayerSpeed  = 10f;  // percents
        private const float StaminaLevelRecoveringPlayerSpeed = 90f;  // percents
        private const float BloodLevelDizzinessLevel          = 30f;  // percents
        private const float BloodLevelBlackoutLevel           = 15f;  // percents
        private const float LowBodyTemperatureDizzinessLevel  = 33f;  // percents
        private const float LowBodyTemperatureBlackoutLevel   = 31f;  // percents
        private const float BloodLevelDeathLevel              = 5f;   // percents
        private const float WaterLevelDeathLevel              = 5f;   // percents
        private const float FoodLevelDeathLevel               = 5f;   // percents
        private const float HighTopBloodPressureLevel         = 137f; // mmHg
        private const float FatigueEffectToStaminaDivider     = 550f; // number (less number means greater fatigue impact on stamina)
        private const float CriticalMaximumHeartRate          = 200f; // bpm
        private const float CriticalMinimumHeartRate          = 35f;  // bpm
        private const float CriticalBloodPressureTop          = 235;  // mmHg
        private const float CriticalBloodPressureBottom       = 154;  // mmHg
        private const float CriticalMaximumBodyTemperature    = 41f;  // C
        private const float CriticalMinimumBodyTemperature    = 31f;  // C
        private const float HotWeatherTemperature             = 30f;  // C
        private const float ExtremelyHotWeatherTemperature    = 38f;  // C
        private const float DangerousBloodPressureTop         = 62f;  // mmHg
        private const float DangerousBloodPressureBottom      = 31f;  // mmHg
        private const float FatigueDizzinessLevel             = 70f;  // percents
        private const float FatigueBlackoutLevel              = 95f;  // percents
        private const float FatigueSleepLevel                 = 154f; // percents
        private const float SedativeMaxFatigueImpact          = 136f; // percents

        /* ------------------------------------------------------------------------------------------------------------------ *
         * ------------------------------------------------ Drains and Gains ------------------------------------------------ *
         * ------------------------------------------------------------------------------------------------------------------ */

        private const float BasicWaterDrainPerSecond                  = 0.005f;    // percents per game second
        private const float AdditionalWaterDrainWhileRunningPerSecond = 0.01f;     // percents per game second
        private const float BasicFoodDrainPerSecond                   = 0.00212f;  // percents per game second
        private const float AdditionalFoodDrainWhileRunningPerSecond  = 0.001f;    // percents per game second
        private const float StaminaRegainRatePerSecond                = 0.2f;      // percents per game second
        private const float StaminaRegainRateWhileWalkingPerSecond    = 0.09f;     // percents per game second
        private const float StaminaDecreaseRateWhileRunningPerSecond  = 0.3f;      // percents per game second
        private const float FatigueIncreaseWhenRunning                = 0.01f;     // percents per game second
        private const float UnconsciousWaterDrainPerSecond            = 0.00062f;  // percents per game second
        private const float UnconsciousFoodDrainPerSecond             = 0.001081f; // percents per game second
        private const float HotWeatherWaterDrainBonus                 = 0.0001f;   // percents per game second
        private const float ExtremelyHotWeatherWaterDrainBonus        = 0.006f;    // percents per game second
        private const float HotWeatherStaminaDrainBonus               = 0.0001f;   // percents per game second
        private const float ExtremelyHotWeatherStaminaDrainBonus      = 0.006f;    // percents per game second
        private const float MaximumBloodRegainRatePerSecond           = 0.0008f;   // percents per game second

        /* ------------------------------------------------------------------------------------------------------------------- *
         * ------------------------------------------------ Events Declarations ---------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------- */

        private readonly EventByChance _diseaseDizzinessEvent;
        private readonly EventByChance _diseaseBlackoutsEvent;
        private readonly EventByChance _diseaseDeathEvent;
        private readonly EventByChance _sneezeEvent;
        private readonly EventByChance _coughEvent;
        private readonly EventByChance _bloodLevelDizzinessEvent;
        private readonly EventByChance _bloodLevelBlackoutsEvent;
        private readonly EventByChance _lowBodyTemperatureDizzinessEvent;
        private readonly EventByChance _lowBodyTemperatureBlackoutsEvent;
        private readonly EventByChance _bloodLevelDeathEvent;
        private readonly EventByChance _dehydrationDeathEvent;
        private readonly EventByChance _starvationDeathEvent;
        private readonly EventByChance _vitalsDeathEvent;
        private readonly EventByChance _overdoseDeathEvent;
        private readonly EventByChance _heartFailureDeathEvent;
        private readonly EventByChance _lsdEffect;
        private readonly EventByChance _fatigueDizzinessEvent;
        private readonly EventByChance _fatigueBlackoutsEvent;
        private readonly EventByChance _fatigueSleepEvent;
        private readonly EventByChance _sedativeSleepEvent;

        private readonly FixedEvent _highPressureEvent;
        private readonly FixedEvent _normalPressureEvent;
        private readonly FixedEvent _drowningEvent;

        /* ------------------------------------------------------------------------------------------------------------------- *
         * ------------------------------------------------------ Fields ----------------------------------------------------- *
         * ------------------------------------------- !!! RESTORE ON SAVE LOAD !!! ------------------------------------------ */

        private DateTime _lastUpdateGameTime;
        private HealthState _healthSnapshot;

        private readonly HealthState _etalonStatus;
        private readonly HealthState _healthyStatus;
        private readonly UnderwaterHealthEffectsController _underwaterEffects;
        private readonly RunningHealthEffectsController _runningEffects;
        private readonly FatigueHealthEffectsController _fatigueEffects;
        private readonly InventoryHealthEffectsController _inventoryEffects;
        private readonly MedicalAgentsSideEffectsController _medicalAgentsEffects;
        private readonly ConsumablesSideEffectsController _consumablesSideEffects;
        private readonly ClothesSideEffectsController _clothesSideEffects;
        private readonly DiseaseMonitorsNode _diseaseMonitors;
        private readonly ActiveMedicalAgentsMonitorsNode _medicalAgentsMonitors;

        internal float _previousDiseaseVitalsChangeRate;
        internal float _previousInjuryVitalsChangeRate;
        private float _healthCheckCooldownTimer = HealthUpdateInterval; // first iteration allowed

        private float _vitalsFluctuateEquilibrium;
        private float _vilalsFluctuateCheckCounter;
        private bool _isHighPressureEventTriggered;
        private float _actualFatigueValue;

        public ActiveMedicalAgentsMonitorsNode Medicine
        {
            get { return _medicalAgentsMonitors; }
        }

        public bool IsSleepDisorder
        {
            get
            {
                if (Status.IsSleepDisorder && !Medicine.IsSedativeActive)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsSleepAllowedByMedicine
        {
            get
            {
                if (Medicine.IsEpinephrineActive ||
                    (Status.FatiguePercentage <= 65f &&
                     (!Medicine.IsSedativeActive ||
                      (Medicine.IsSedativeActive &&
                       Medicine.SedativeAgent.PercentOfActivity < 50f))))
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsSleepAllowed
        {
            get { return IsSleepAllowedByMedicine && !IsSleepDisorder; }
        }

        public bool IsTired
        {
            get { return Status.FatiguePercentage >= 55f && Status.FatiguePercentage < 80f && IsSleepAllowed; }
        }

        public bool IsExhausted
        {
            get { return Status.FatiguePercentage >= 80f && Status.FatiguePercentage < 100f && IsSleepAllowed; }
        }

        public bool IsExtremelyExhausted
        {
            get { return Status.FatiguePercentage >= 100f && IsSleepAllowed; }
        }

        public bool IsInventoryOverload
        {
            get { return _inventoryEffects.IsFreezed; }
        }

        public bool UnconsciousMode { get; set; }


        public enum CoughLevels {
            CoughLight,
            CoughMedium,
            CoughBad
        }

        public HealthController(IGameController gc) { 
            _gc = gc;

            _underwaterEffects = new UnderwaterHealthEffectsController(gc, this);
            _runningEffects = new RunningHealthEffectsController(gc, this);
            _fatigueEffects = new FatigueHealthEffectsController(gc, this);
            _inventoryEffects = new InventoryHealthEffectsController(gc);
            _medicalAgentsEffects = new MedicalAgentsSideEffectsController(gc);
            _consumablesSideEffects = new ConsumablesSideEffectsController(gc);
            _clothesSideEffects = new ClothesSideEffectsController(gc);
            _diseaseMonitors = new DiseaseMonitorsNode(gc);
            _medicalAgentsMonitors = new ActiveMedicalAgentsMonitorsNode(gc);

            _healthyStatus = new HealthState {
                StaminaPercentage = 100,
                WaterPercentage = 80,
                BloodPercentage = 73,
                BloodPressureTop = 126,
                BloodPressureBottom = 71,
                BodyTemperature = 36.7f,
                FoodPercentage = 68,
                HeartRate = 69f,
                FatiguePercentage = 0,
                OxygenPercentage = 100
            };

            // Default health state
            Status = _healthyStatus.Clone(null);
            _etalonStatus = _healthyStatus.Clone(null);

            _healthSnapshot = Status.Clone(null);

            /******* VFX events produced by the Health Engine *******/

            _diseaseDizzinessEvent            = new EventByChance("Disease dizziness check"             , ev => Events.NotifyAll(l => l.DiseaseDizziness()), DiseaseDizzinessCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _diseaseBlackoutsEvent            = new EventByChance("Disease blackouts check"             , ev => Events.NotifyAll(l => l.DiseaseBlackout()), DiseaseBlackoutsCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _lowBodyTemperatureDizzinessEvent = new EventByChance("Low body temperature dizziness check", ev => Events.NotifyAll(l => l.LowBodyTemperatureDizziness()), LowBodyTemperatureDizzinessCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _lowBodyTemperatureBlackoutsEvent = new EventByChance("Low body temperature blackouts check", ev => Events.NotifyAll(l => l.LowBodyTemperatureBlackout()), LowBodyTemperatureBlackoutsCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _bloodLevelDizzinessEvent         = new EventByChance("Blood level dizziness check"         , ev => Events.NotifyAll(l => l.LowBloodLevelDizziness()), BloodLevelDizzinessChance, BloodLevelDizzinessCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _bloodLevelBlackoutsEvent         = new EventByChance("Blood level blackouts check"         , ev => Events.NotifyAll(l => l.LowBloodLevelBlackout()), BloodLevelBlackoutChance, BloodLevelBlackoutsCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _lsdEffect                        = new EventByChance("LSD effect check"                    , ev => Events.NotifyAll(l => l.OverdoseEffect()), LsdEffectChance, LsdEffectCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _fatigueDizzinessEvent            = new EventByChance("Fatigue dizziness check"             , ev => Events.NotifyAll(l => l.FatigueDizziness()), FatigueDizzinessChance, FatigueDizzinessCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _fatigueBlackoutsEvent            = new EventByChance("Fatigue blackouts check"             , ev => Events.NotifyAll(l => l.FatigueBlackout()), FatigueBlackoutChance, FatigueBlackoutsCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _fatigueSleepEvent                = new EventByChance("Extreme fatigue sleep check"         , ev => Events.NotifyAll(l => l.ExtremeFatigueSleepTrigger()), FatigueSleepChance, FatigueSleepCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _sedativeSleepEvent               = new EventByChance("Sedative extreme sleep check"        , ev => Events.NotifyAll(l => l.SedativeSleepTrigger()), SedativeSleepChance, SedativeSleepCheckInterval, HealthUpdateInterval) { AutoReset = true };

            /******* Health death events (deaths from natural causes) *******/

            _diseaseDeathEvent                = new EventByChance("Disease death check"      , ev => Events.NotifyAll(l => l.DeathFromDisease()), DiseaseDeathCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _vitalsDeathEvent                 = new EventByChance("Vitals death check"       , ev => Events.NotifyAll(l => l.DeathFromBadVitals()), VitalsDeathChance, VitalsDeathCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _overdoseDeathEvent               = new EventByChance("Overdose death check"     , ev => Events.NotifyAll(l => l.DeathByOverdose()), OverdoseDeathChance, OverdoseDeathCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _heartFailureDeathEvent           = new EventByChance("Heart failure death check", ev => Events.NotifyAll(l => l.DeathByHeartFailure()), HeartFailureDeathChance, HeartFailureDeathCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _bloodLevelDeathEvent             = new EventByChance("Blood level death check"  , ev => Events.NotifyAll(l => l.DeathByBloodLoss()), BloodLevelDeathChance, BloodLevelDeathCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _dehydrationDeathEvent            = new EventByChance("Dehydration death check"  , ev => Events.NotifyAll(l => l.DeathByDehydration()), DehydrationDeathChance, DehydrationDeathCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _starvationDeathEvent             = new EventByChance("Starvation death check"   , ev => Events.NotifyAll(l => l.DeathByStarvation()), StarvationDeathChance, StarvationDeathCheckInterval, HealthUpdateInterval) { AutoReset = true };

            /******* SFX events produced by the Health Engine *******/

            _sneezeEvent = new EventByChance("Sneeze check", ev => Events.NotifyAll(l => l.Sneeze()), SneezeCheckInterval, HealthUpdateInterval) { AutoReset = true };
            _coughEvent  = new EventByChance("Cough check" , ev => Events.NotifyAll(l => l.Cough((CoughLevels)ev.Param)), CoughCheckInterval, HealthUpdateInterval) { AutoReset = true };

            /******* Other events produced by the Health Engine *******/

            _highPressureEvent   = new FixedEvent("High pressure trigger"  , ev => Events.NotifyAll(l => l.HighBloodPressureTriggeredOn())) { AutoReset = true };
            _normalPressureEvent = new FixedEvent("Normal pressure trigger", ev => Events.NotifyAll(l => l.HighBloodPressureTriggeredOff())) { AutoReset = true };
            _drowningEvent       = new FixedEvent("Drowning", ev => Events.NotifyAll(l => l.StartDrowning())) { AutoReset = true };
        }

        public void Initialize() {
            _clothesSideEffects.Initialize();
        }

        public HealthState Status { get; set; }

        public void Check(float deltaTime) {
            if (!_gc.WorldTime.HasValue)
                return;

            HealthState newState = null;

            if (!UnconsciousMode) {
                // We do not check health status more than once per HealthUpdateInterval real seconds
                if (_healthCheckCooldownTimer < HealthUpdateInterval) {
                    _healthCheckCooldownTimer += deltaTime;
                    return;
                }

                if (_healthCheckCooldownTimer >= HealthUpdateInterval)
                    _healthCheckCooldownTimer = 0;
            } else {
                // When sleeping, we check health state more often
                if (_healthCheckCooldownTimer < UnconsciousHealthUpdateInterval) {
                    _healthCheckCooldownTimer += deltaTime;
                    return;
                }

                if (_healthCheckCooldownTimer >= UnconsciousHealthUpdateInterval)
                    _healthCheckCooldownTimer = 0;
            }

            // Every HealthUpdateInterval or UnconsciousHealthUpdateInterval seconds

            if (_lastUpdateGameTime == default(DateTime))
                _lastUpdateGameTime = _gc.WorldTime.Value;

            var gameSecondsSinceLastCall = (float)(_gc.WorldTime.Value - _lastUpdateGameTime).TotalSeconds;

            _lastUpdateGameTime = _gc.WorldTime.Value;

            // Side effects update
            _underwaterEffects.Update(gameSecondsSinceLastCall, deltaTime);
            _runningEffects.Update(gameSecondsSinceLastCall, deltaTime);
            _inventoryEffects.Update(deltaTime);
            _medicalAgentsEffects.Check();
            _consumablesSideEffects.Check();
            _clothesSideEffects.Check();

            newState = Status.Clone(_gc.WorldTime);

            #region Real-time vitals

            var staminaDrainValue = GetStaminaDrainRate() * gameSecondsSinceLastCall;
            var waterDrainValue = GetWaterDrainRate() * gameSecondsSinceLastCall;
            var foodDrainValue = GetFoodDrainRate() * gameSecondsSinceLastCall;

            newState.SetStaminaLevel(Status.StaminaPercentage - staminaDrainValue);
            newState.SetWaterLevel(Status.WaterPercentage - waterDrainValue);
            newState.SetFoodLevel(Status.FoodPercentage - foodDrainValue);

            if (newState.WaterPercentage < 0f)
                newState.SetWaterLevel(0f);

            if (newState.FoodPercentage < 0f)
                newState.SetFoodLevel(0f);

            if (newState.StaminaPercentage < 0f)
                newState.SetStaminaLevel(0f);

            newState.HeartRate = _healthyStatus.HeartRate;
            newState.BodyTemperature = _healthyStatus.BodyTemperature;
            newState.BloodPressureTop = _healthyStatus.BloodPressureTop;
            newState.BloodPressureBottom = _healthyStatus.BloodPressureBottom;
            newState.OxygenPercentage = _healthyStatus.OxygenPercentage;

            AddSideEffectsVitalsBonuses(newState);
            ProcessStaminaEffects(newState, gameSecondsSinceLastCall, deltaTime);

            #endregion

            if (newState.CheckTime == default (DateTime)) {
                newState.CheckTime = _gc.WorldTime.Value;
                newState.LastSleepTime = _gc.WorldTime.Value;
            }

            _diseaseMonitors.Check();
            _medicalAgentsMonitors.Check();

            FluctuateVitals(_healthyStatus);

            newState.IsBloodLoss = false; // Will be set to true later here if needed
            newState.IsActiveInjury = false; // Will be set to true later here if needed
            newState.IsActiveDisease = false; // Will be set to true later here if needed
            newState.IsFoodDisgust = false; // Will be set to true later here if needed
            newState.IsSleepDisorder = false; // Will be set to true later here if needed
            newState.CannotRun = false; // Will be set to true later here if needed
            newState.IsLegFracture = false; // Will be set to true later here if needed
            newState.ActiveDiseasesWorstLevel = DiseaseLevels.InitialStage; // Will be set to the actual level later here if needed

            var gameSecondsSinceLastCheck = (float)(_gc.WorldTime.Value - newState.CheckTime).TotalSeconds;

            _actualFatigueValue += (gameSecondsSinceLastCheck / (3600f * 24)) * 100;

            if (Medicine.IsEpinephrineActive)
            {
                newState.FatiguePercentage = _actualFatigueValue * (1f - Medicine.EpinephrineAgent.PercentOfActivity / 100f);
            }
            else
            {
                newState.FatiguePercentage = _actualFatigueValue;
            }

            if (Medicine.IsSedativeActive)
            {
                newState.FatiguePercentage = _actualFatigueValue + SedativeMaxFatigueImpact * (Medicine.SedativeAgent.ActiveDosesCount / (float)SedativeDosesUntilMaxFatigueImpact) * (Medicine.SedativeAgent.PercentOfActivity / 100f);
            }
            else
            {
                newState.FatiguePercentage = _actualFatigueValue;
            }

            newState.CheckTime = _gc.WorldTime.Value;

            _fatigueEffects.SlowUpdate();

            #region Diseases

            List<DiseaseStage> activeStages = null;

            // New disease can appear here, after _diseaseMonitors.Check() call, already After the cloning 
            var actualDiseases = Status.GetActualDiseases(_gc.WorldTime.Value);

            newState.ActiveDiseases = actualDiseases;

            if (newState.ActiveDiseases.Count > 0) {
                activeStages = new List<DiseaseStage>();

                newState.ActiveDiseases.ForEach (x => {
                    var stage = x.GetActiveStage(_gc.WorldTime.Value);

                    if (stage != null) {
                        x.Disease.Check(x, _gc);
                        activeStages.Add(stage);
                    }
                });
            }

            var superStage = CombineStages(activeStages);

            if (superStage != null) {
                newState.IsActiveDisease = true;
                newState.ActiveDiseasesWorstLevel = superStage.Level;
                newState.IsFoodDisgust = superStage.CannotEat;
                newState.IsSleepDisorder = superStage.CannotSleep;
                newState.CannotRun = superStage.CannotRun;

                ProcessDiseaseVitals(superStage, newState, gameSecondsSinceLastCheck);
                AddSideEffectsVitalsBonuses(newState);
                ProcessDiseaseLevelEffects(superStage);
                ProcessAdditionalDiseaseEffects(superStage, deltaTime);
            }

            #endregion

            #region Injuries

            // Re-asking active injuries -- just in case
            var activeInjuries = new List<Tuple<InjuryStage, ActiveInjury>>();
            var actualInjuries = Status.GetActualInjuries(_gc.WorldTime.Value);

            newState.ActiveInjuries = actualInjuries;

            if (newState.ActiveInjuries.Count > 0) {
                foreach (var activeInjury in newState.ActiveInjuries)
                {
                    if (!activeInjury.IsTreated)
                    {
                        var stage = activeInjury.GetActiveStage(_gc.WorldTime.Value);

                        if(stage != null)
                            activeInjuries.Add(new Tuple<InjuryStage, ActiveInjury>(stage, activeInjury));
                    }
                }
            }

            TriggerNeededDiseases(newState, activeInjuries);

            var superInjury = CombineInjuries(activeInjuries);

            if (superInjury != null) {
                newState.IsActiveInjury = true;
                newState.IsLegFracture = activeInjuries.Any(x => x.Item1.IsFracture && 
                                          (x.Item2.BodyPart == BodyParts.LeftKnee || x.Item2.BodyPart == BodyParts.RightKnee ||
                                           x.Item2.BodyPart == BodyParts.LeftShin || x.Item2.BodyPart == BodyParts.RightShin ||
                                           x.Item2.BodyPart == BodyParts.LeftHip  || x.Item2.BodyPart == BodyParts.RightHip  ||
                                           x.Item2.BodyPart == BodyParts.LeftFoot || x.Item2.BodyPart == BodyParts.RightFoot));

                ProcessInjuryVitals(superInjury, newState, gameSecondsSinceLastCheck);
            }

            #endregion

            #region Blood Regain

            newState.BloodPercentage += Helpers.Lerp(0f, MaximumBloodRegainRatePerSecond, newState.FoodPercentage / 100f);

            #endregion

            ProcessCommonEffects(newState, deltaTime);
            ProcessFatigueEffects(newState, deltaTime);
            ProcessSedativeEffects(deltaTime);

            // Check for deaths from natural causes 
            CheckVitalsDeath(newState, deltaTime);
            CheckCommonDeath(newState, deltaTime);
            CheckDiseaseDeath(superStage, deltaTime);
            CheckOverdoseDeath(deltaTime);
            CheckHeartFailureDeath(deltaTime);

            if(UnconsciousMode)
                newState.FatiguePercentage = Status.FatiguePercentage;

            // We're done with all checks
            Status = newState;
        }

        #region Side Effects

        private void AddSideEffectsVitalsBonuses(HealthState state) {
            state.BloodPressureTop += _underwaterEffects.BloodPressureTopBonus;
            state.BloodPressureBottom += _underwaterEffects.BloodPressureBottomBonus;
            state.HeartRate += _underwaterEffects.HeartRateBonus;
            state.OxygenPercentage += _underwaterEffects.OxygenLevelBonus;

            state.BodyTemperature += _runningEffects.BodyTemperatureBonus;
            state.BloodPressureTop += _runningEffects.BloodPressureTopBonus;
            state.BloodPressureBottom += _runningEffects.BloodPressureBottomBonus;
            state.HeartRate += _runningEffects.HeartRateBonus;
            state.OxygenPercentage += _runningEffects.OxygenLevelBonus;

            state.BloodPressureTop += _fatigueEffects.BloodPressureTopBonus;
            state.BloodPressureBottom += _fatigueEffects.BloodPressureBottomBonus;
            state.HeartRate += _fatigueEffects.HeartRateBonus;

            state.BodyTemperature += _medicalAgentsEffects.BodyTemperatureBonus;
            state.BloodPressureTop += _medicalAgentsEffects.BloodPressureTopBonus;
            state.BloodPressureBottom += _medicalAgentsEffects.BloodPressureBottomBonus;
            state.HeartRate += _medicalAgentsEffects.HeartRateBonus;

            state.BloodPressureTop += _consumablesSideEffects.BloodPressureTopBonus;
            state.BloodPressureBottom += _consumablesSideEffects.BloodPressureBottomBonus;
            state.HeartRate += _consumablesSideEffects.HeartRateBonus;

            state.BodyTemperature += _clothesSideEffects.BodyTemperatureBonus;
            state.HeartRate += _clothesSideEffects.HeartRateBonus;

            if (state.OxygenPercentage > 100f)
                state.OxygenPercentage = 100f;
        }

        private void TakeAwaySideEffectsVitalsBonuses(HealthState state) {
            state.BloodPressureTop -= _underwaterEffects.BloodPressureTopBonus;
            state.BloodPressureBottom -= _underwaterEffects.BloodPressureBottomBonus;
            state.HeartRate -= _underwaterEffects.HeartRateBonus;
            state.OxygenPercentage -= _underwaterEffects.OxygenLevelBonus;

            state.BodyTemperature -= _runningEffects.BodyTemperatureBonus;
            state.BloodPressureTop -= _runningEffects.BloodPressureTopBonus;
            state.BloodPressureBottom -= _runningEffects.BloodPressureBottomBonus;
            state.HeartRate -= _runningEffects.HeartRateBonus;
            state.OxygenPercentage += _runningEffects.OxygenLevelBonus;

            state.BloodPressureTop -= _fatigueEffects.BloodPressureTopBonus;
            state.BloodPressureBottom -= _fatigueEffects.BloodPressureBottomBonus;
            state.HeartRate -= _fatigueEffects.HeartRateBonus;

            state.BodyTemperature -= _medicalAgentsEffects.BodyTemperatureBonus;
            state.BloodPressureTop -= _medicalAgentsEffects.BloodPressureTopBonus;
            state.BloodPressureBottom -= _medicalAgentsEffects.BloodPressureBottomBonus;
            state.HeartRate -= _medicalAgentsEffects.HeartRateBonus;

            state.BloodPressureTop -= _consumablesSideEffects.BloodPressureTopBonus;
            state.BloodPressureBottom -= _consumablesSideEffects.BloodPressureBottomBonus;
            state.HeartRate -= _consumablesSideEffects.HeartRateBonus;

            state.BodyTemperature -= _clothesSideEffects.BodyTemperatureBonus;
            state.HeartRate -= _clothesSideEffects.HeartRateBonus;

            if (state.OxygenPercentage < 0f)
                state.OxygenPercentage = 0f;
        }

        #endregion

        #region Processing vitals and gameplay effects

        internal void SetFatiguePercentage(float v)
        {
            _actualFatigueValue = v;
            Status.FatiguePercentage = v;
        }

        private void FluctuateVitals(HealthState newState) {
            if (UnconsciousMode)
                return;

            _vilalsFluctuateCheckCounter++;

            if (_vilalsFluctuateCheckCounter <= VitalsFluctuateEveryNChecks)
                return;

            _vilalsFluctuateCheckCounter = 0;

            float multiplier;

            if (50.WillHappen()) {
                multiplier = 1;
            } else {
                multiplier = -1;
            }

            _vitalsFluctuateEquilibrium += multiplier;

            if (Math.Abs(_vitalsFluctuateEquilibrium) > VitalsFluctuateEquilibriumMargin)
                return;

            newState.HeartRate += HeartRateFluctuationDelta * multiplier;
            newState.BodyTemperature -= BodyTemperatureFluctuationDelta * multiplier;
            newState.BloodPressureTop += BloodPressureTopFluctuationDelta * multiplier;
            newState.BloodPressureBottom -= BloodPressureBottomFluctuationDelta * multiplier;

            if (Math.Abs(_etalonStatus.HeartRate - newState.HeartRate) > VitalsFluctuateEquilibriumMargin * HeartRateFluctuationDelta)
                newState.HeartRate -= HeartRateFluctuationDelta * multiplier;

            if (Math.Abs(_etalonStatus.BodyTemperature - newState.BodyTemperature) > VitalsFluctuateEquilibriumMargin * BodyTemperatureFluctuationDelta)
                newState.BodyTemperature += BodyTemperatureFluctuationDelta * multiplier;

            if (Math.Abs(_etalonStatus.BloodPressureTop - newState.BloodPressureTop) > VitalsFluctuateEquilibriumMargin * BloodPressureTopFluctuationDelta)
                newState.BloodPressureTop -= BloodPressureTopFluctuationDelta * multiplier;

            if (Math.Abs(_etalonStatus.BloodPressureBottom - newState.BloodPressureBottom) > VitalsFluctuateEquilibriumMargin * BloodPressureBottomFluctuationDelta)
                newState.BloodPressureBottom += BloodPressureBottomFluctuationDelta * multiplier;
        }

        private void TriggerNeededDiseases(HealthState newState, List<Tuple<InjuryStage, ActiveInjury>> activeInjuries) {
            if (activeInjuries == null)
                return;

            if (!activeInjuries.Any())
                return;

            foreach (var injury in activeInjuries) {
                if (!injury.Item2.IsDiseaseProbabilityChecked && injury.Item1.TriggeringDisease != null && !injury.Item1.IsDiseaseTriggered)
                {
                    // We check disease tiggering only once per injury
                    injury.Item2.IsDiseaseProbabilityChecked = true;

                    if (injury.Item1.DiseaseProbability.WillHappen())
                    {
                        injury.Item1.IsDiseaseTriggered = true;
                        newState.ActiveDiseases.Add(new ActiveDisease(_gc, injury.Item1.TriggeringDisease, injury.Item2, _gc.WorldTime.Value));
                    }
                }
            }
        }

        private void ProcessStaminaEffects(HealthState state, float gameSecondsSinceLastCall, float deltaTime) {
            if (UnconsciousMode)
                return;

            if (state.StaminaPercentage < 0.001f && (_gc.Player.IsSwimming || _gc.Player.IsUnderWater))
            {
                // Drown
                _drowningEvent.Invoke(deltaTime);
            }

            // Reading cached value here
            if (Medicine.IsEpinephrineActive && Medicine.EpinephrineAgent.PercentOfActivity >= 50f)
            {
                Events.NotifyAll(l => l.MovementSpeedChange(HealthyRunSpeed,HealthyWalkSpeed,HealthyCrouchSpeed));

                return;
            }

            // Leg is broken
            if (Status.IsLegFracture)
            {
                //("Player speed reduced by leg fracture");

                Events.NotifyAll(l => l.MovementSpeedChange(CriticalCrouchSpeed,CriticalCrouchSpeed,CriticalCrouchSpeed));

                return;
            }

            // Too sick to move normally
            if (Status.CannotRun)
            {
                Events.NotifyAll(l => l.MovementSpeedChange(CriticalRunSpeed,CriticalWalkSpeed,CriticalCrouchSpeed));

                return;
            }

            if (!_gc.Player.IsWalking && !_gc.Player.IsStanding) {
                // Fatigue increase when running
                if (gameSecondsSinceLastCall > 0f)
                {
                    _actualFatigueValue += FatigueIncreaseWhenRunning * gameSecondsSinceLastCall;
                    state.FatiguePercentage += FatigueIncreaseWhenRunning * gameSecondsSinceLastCall;
                }
            }

            if (Status.StaminaPercentage < StaminaLevelAffectingPlayerSpeed) {
                //("Player speed reduced by stamina");

                Events.NotifyAll(l => l.MovementSpeedChange(CriticalRunSpeed,CriticalWalkSpeed,CriticalCrouchSpeed));
            } else {
                if (state.FatiguePercentage > StaminaLevelRecoveringPlayerSpeed) {
                    //("Player speed reduced by fatigue");

                    Events.NotifyAll(l => l.MovementSpeedChange(CriticalRunSpeed,CriticalWalkSpeed,CriticalCrouchSpeed));
                } else {
                    Events.NotifyAll(l => l.MovementSpeedChange(HealthyRunSpeed,HealthyWalkSpeed,HealthyCrouchSpeed));
                }
            }

            if (_inventoryEffects.IsFreezed) {
                Events.NotifyAll(l => l.MovementSpeedChange(0f,0f,0f));
            } else {
                Events.NotifyAll(l => l.ApplyMovementSpeedDelta(_inventoryEffects.PlayerRunSpeedBonus,_inventoryEffects.PlayerWalkSpeedBonus,_inventoryEffects.PlayerCrouchSpeedBonus));
            }

            Events.NotifyAll(l => l.ApplyMovementSpeedDelta(_clothesSideEffects.PlayerRunSpeedBonus,null,null));
        }

        private void ProcessDiseaseLevelEffects(DiseaseStage superStage) {
            if (superStage.CannotRun || superStage.Level == DiseaseLevels.Critical) {
                //("Player speed reduced by disease");

                Events.NotifyAll(l => l.MovementSpeedChange(CriticalRunSpeed,CriticalWalkSpeed,CriticalCrouchSpeed));
            }
        }

        private void ProcessDiseaseVitals(DiseaseStage superStage, HealthState newState, float gameSecondsSinceLastCall) {
            if (_gc.WorldTime.Value > superStage.WillEndAt.Value)
                return;

            var timeDelta = _gc.WorldTime.Value - superStage.WillTriggerAt.Value;
            var vitalsDelta = superStage.VitalsTargetSeconds.Value;
            var vitalsChangeRate = (float) timeDelta.TotalSeconds / vitalsDelta;

            if (_previousDiseaseVitalsChangeRate < 1f && vitalsChangeRate >= 1f || vitalsChangeRate < _previousDiseaseVitalsChangeRate) {
                // Take vitals snapshot for lerping
                _healthSnapshot = Status.Clone(_gc.WorldTime);

                // Take away bonus effects for a snapshot. We need 'clean' one.
                TakeAwaySideEffectsVitalsBonuses(_healthSnapshot);

                //("Health snapshot taken");
            }

            _previousDiseaseVitalsChangeRate = vitalsChangeRate;

            if (superStage.TargetBodyTemperature.HasValue)
                newState.BodyTemperature = Helpers.Lerp(_healthSnapshot.BodyTemperature, superStage.TargetBodyTemperature.Value, vitalsChangeRate);

            if (superStage.TargetHeartRate.HasValue)
                newState.HeartRate = Helpers.Lerp(_healthSnapshot.HeartRate, superStage.TargetHeartRate.Value, vitalsChangeRate);

            if (superStage.TargetBloodPressureTop.HasValue)
                newState.BloodPressureTop = Helpers.Lerp(_healthSnapshot.BloodPressureTop, superStage.TargetBloodPressureTop.Value, vitalsChangeRate);

            if (superStage.TargetBloodPressureBottom.HasValue)
                newState.BloodPressureBottom = Helpers.Lerp(_healthSnapshot.BloodPressureBottom, superStage.TargetBloodPressureBottom.Value, vitalsChangeRate);

            if (!UnconsciousMode) {
                if (superStage.StaminaDrainPerSecond.HasValue)
                    newState.SetStaminaLevel(newState.StaminaPercentage -= superStage.StaminaDrainPerSecond.Value * gameSecondsSinceLastCall);

                if (superStage.FoodDrainPerSecond.HasValue)
                    newState.SetFoodLevel(newState.FoodPercentage -= superStage.FoodDrainPerSecond.Value * gameSecondsSinceLastCall);

                if (superStage.WaterDrainPerSecond.HasValue)
                    newState.SetWaterLevel(newState.WaterPercentage - superStage.WaterDrainPerSecond.Value * gameSecondsSinceLastCall);

                if (superStage.FatigueIncreasePerSecond.HasValue)
                {
                    _actualFatigueValue += superStage.FatigueIncreasePerSecond.Value * gameSecondsSinceLastCall;
                    newState.FatiguePercentage += superStage.FatigueIncreasePerSecond.Value * gameSecondsSinceLastCall;
                }
            }
        }

        private void ProcessInjuryVitals(InjuryStage superInjury, HealthState newState, float gameSecondsSinceLastCall) {
            var timeDelta = _gc.WorldTime.Value - superInjury.WillTriggerAt.Value;
            var vitalsDelta = superInjury.StageDuration.TotalSeconds;
            var vitalsChangeRate = (float) timeDelta.TotalSeconds / (float) vitalsDelta;

            _previousInjuryVitalsChangeRate = vitalsChangeRate;

            if (superInjury.BloodDrainPerSecond.HasValue && superInjury.BloodDrainPerSecond.Value > 0f) {
                newState.BloodPercentage -= superInjury.BloodDrainPerSecond.Value * gameSecondsSinceLastCall;
                newState.IsBloodLoss = true;
            }

            if (superInjury.StaminaDrainPerSecond.HasValue)
                newState.StaminaPercentage -= superInjury.StaminaDrainPerSecond.Value * gameSecondsSinceLastCall;

            if (superInjury.FatigueIncreasePerSecond.HasValue)
            {
                _actualFatigueValue += superInjury.FatigueIncreasePerSecond.Value * gameSecondsSinceLastCall;
                newState.FatiguePercentage += superInjury.FatigueIncreasePerSecond.Value * gameSecondsSinceLastCall;
            }

            if (superInjury.WalkSpeedDecrease > 0) {
                Events.NotifyAll(l => l.ApplyMovementSpeedDelta(-superInjury.WalkSpeedDecrease,-superInjury.WalkSpeedDecrease,null));
            }

            Events.NotifyAll(l => l.ReportLimpingState(newState.IsLegFracture));
        }

        #endregion

        #region Checking Events Chances

        private void CheckVitalsDeath(HealthState newState, float deltaTime) {
            if (newState.BloodPressureTop <= DangerousBloodPressureTop || newState.BloodPressureBottom <= DangerousBloodPressureBottom ||
                newState.BloodPressureTop > CriticalBloodPressureTop || newState.OxygenPercentage <= 0f ||
                newState.HeartRate > CriticalMaximumHeartRate || newState.HeartRate <= CriticalMinimumHeartRate || 
                newState.BloodPressureBottom > CriticalBloodPressureBottom || newState.BodyTemperature > CriticalMaximumBodyTemperature || newState.BodyTemperature <= CriticalMinimumBodyTemperature) {
                _vitalsDeathEvent.Check(deltaTime);
            }
        }

        private void CheckCommonDeath(HealthState newState, float deltaTime)
        {
            if (newState.BloodPercentage < BloodLevelDeathLevel)
            {
                // Bloood death chance
                _bloodLevelDeathEvent.Check(deltaTime);
            }

            if (newState.WaterPercentage < WaterLevelDeathLevel)
            {
                // Dehydration death chance
                _dehydrationDeathEvent.Check(deltaTime);
            }

            if (newState.FoodPercentage < FoodLevelDeathLevel)
            {
                // Starvation death chance
                _starvationDeathEvent.Check(deltaTime);
            }
        }

        private void ProcessCommonEffects(HealthState newState, float deltaTime)
        {
            if (newState.BloodPressureTop > HighTopBloodPressureLevel)
            {
                if (!_isHighPressureEventTriggered)
                {
                    _highPressureEvent.Invoke(deltaTime);
                    _isHighPressureEventTriggered = true;
                }
            }
            else
            {
                if (newState.BloodPressureTop < HighTopBloodPressureLevel)
                {
                    if (_isHighPressureEventTriggered)
                    {
                        _normalPressureEvent.Invoke(deltaTime);
                        _isHighPressureEventTriggered = false;
                    }
                }
            }

            if (newState.BodyTemperature < LowBodyTemperatureDizzinessLevel)
            {
                // Low body temperature dizziness chance
                _lowBodyTemperatureDizzinessEvent.Check(deltaTime);
            }

            if (newState.BodyTemperature < LowBodyTemperatureBlackoutLevel)
            {
                // Low body temperature blackout chance
                _lowBodyTemperatureBlackoutsEvent.Check(deltaTime);
            }

            if (newState.BloodPercentage < BloodLevelDizzinessLevel)
            {
                // Bloood dizziness chance
                _bloodLevelDizzinessEvent.Check(deltaTime);
            }

            if (newState.BloodPercentage < BloodLevelBlackoutLevel)
            {
                // Bloood blackout chance
                _bloodLevelBlackoutsEvent.Check(deltaTime);
            }

            if (_gc.Health.Medicine.IsMorphineActive && _gc.Health.Medicine.MorphineAgent.ActiveDosesCount >= MorphineLsdEffectValue)
            {
                // LSD visual effect chance
                _lsdEffect.Check(deltaTime);
            }
        }

        private void ProcessFatigueEffects(HealthState newState, float deltaTime)
        {
            if (newState.FatiguePercentage > FatigueDizzinessLevel)
            {
                // Fatigue dizziness chance
                _fatigueDizzinessEvent.Check(deltaTime);
            }

            if (newState.FatiguePercentage > FatigueBlackoutLevel)
            {
                // Fatigue blackouts chance
                _fatigueBlackoutsEvent.Check(deltaTime);
            }

            if (newState.FatiguePercentage > FatigueSleepLevel)
            {
                // Fatigue sleep chance
                _fatigueSleepEvent.Check(deltaTime);
            }
        }

        private void ProcessSedativeEffects(float deltaTime)
        {
            if (_gc.Health.Medicine.IsSedativeActive && _gc.Health.Medicine.SedativeAgent.ActiveDosesCount >= SedativeForceSleepValue)
            {
                // Sedative force-sleep chance
                _sedativeSleepEvent.Check(deltaTime);
            }
        }

        private void ProcessAdditionalDiseaseEffects(DiseaseStage superStage, float deltaTime) {
            if (superStage.BlackoutChance > 0) {
                // Disease blackouts chance
                _diseaseBlackoutsEvent.Check(superStage.BlackoutChance, deltaTime);
            }

            if (superStage.DizzinessChance > 0) {
                // Disease dizziness chance
                _diseaseDizzinessEvent.Check(superStage.DizzinessChance, deltaTime);
            }

            if (superStage.SneezeChance > 0) {
                // Sneeze chance
                _sneezeEvent.Check(superStage.SneezeChance, deltaTime);
            }

            if (superStage.CoughChance > 0) {
                // Cough chance
                if (superStage.CoughChance <= CoughChanceLight)
                    _coughEvent.Param = (int)CoughLevels.CoughLight;

                if (superStage.CoughChance > CoughChanceLight && superStage.CoughChance <= CoughChanceMedium)
                    _coughEvent.Param = (int)CoughLevels.CoughMedium;

                if (superStage.CoughChance > CoughChanceMedium)
                    _coughEvent.Param = (int)CoughLevels.CoughBad;

                _coughEvent.Check(superStage.CoughChance, deltaTime);
            }
        }

        private void CheckDiseaseDeath(DiseaseStage superStage, float deltaTime)
        {
            if (superStage == null)
                return;

            if (superStage.ChanceOfDeath > 0)
            {
                // Disease death chance
                _diseaseDeathEvent.Check(superStage.ChanceOfDeath, deltaTime);
            }
        }

        private void CheckOverdoseDeath(float deltaTime)
        {
            MedicalConsumablesGroup groupFiredOverdose = null;

            if (_gc.Health.Medicine.IsEpinephrineActive && _gc.Health.Medicine.EpinephrineAgent.ActiveDosesCount >= EpinephrineOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.EpinephrineAgent.MedicalGroup;
            }
            else if (_gc.Health.Medicine.IsAntiVenomActive && _gc.Health.Medicine.AntiVenomAgent.ActiveDosesCount >= AntiVenomOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.AntiVenomAgent.MedicalGroup;
            }
            else if (_gc.Health.Medicine.IsAtropineActive && _gc.Health.Medicine.AtropineAgent.ActiveDosesCount >= AtropineOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.AtropineAgent.MedicalGroup;
            }
            else if (_gc.Health.Medicine.IsMorphineActive && _gc.Health.Medicine.MorphineAgent.ActiveDosesCount >= MorphineOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.MorphineAgent.MedicalGroup;
            }
            else if (_gc.Health.Medicine.IsAntibioticActive && _gc.Health.Medicine.AntibioticAgent.ActiveDosesCount >= AntibioticOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.AntibioticAgent.MedicalGroup;
            }
            else if (_gc.Health.Medicine.IsAspirinActive && _gc.Health.Medicine.AspirinAgent.ActiveDosesCount >= AspirinOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.AspirinAgent.MedicalGroup;
            }
            else if (_gc.Health.Medicine.IsAcetaminophenActive && _gc.Health.Medicine.AcetaminophenAgent.ActiveDosesCount >= AcetaminophenOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.AcetaminophenAgent.MedicalGroup;
            }
            else if (_gc.Health.Medicine.IsLoperamideActive && _gc.Health.Medicine.LoperamideAgent.ActiveDosesCount >= LoperamideOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.LoperamideAgent.MedicalGroup;
            }
            else if (_gc.Health.Medicine.IsOseltamivirActive && _gc.Health.Medicine.OseltamivirAgent.ActiveDosesCount >= OseltamivirOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.OseltamivirAgent.MedicalGroup;
            }
            else if (_gc.Health.Medicine.IsSedativeActive && _gc.Health.Medicine.SedativeAgent.ActiveDosesCount >= SedativeOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.SedativeAgent.MedicalGroup;
            }
            else if (_gc.Health.Medicine.IsDoripenemActive && _gc.Health.Medicine.DoripenemAgent.ActiveDosesCount >= DoripomenOverdoseValue)
            {
                groupFiredOverdose = _gc.Health.Medicine.DoripenemAgent.MedicalGroup;
            }

            if (groupFiredOverdose != null)
            {
                // Overdose death chance
                _overdoseDeathEvent.Param = groupFiredOverdose.Name;
                _overdoseDeathEvent.Check(deltaTime);
            }
        }

        private void CheckHeartFailureDeath(float deltaTime)
        {
            if ((_gc.Health.Medicine.IsEpinephrineActive && _gc.Health.Medicine.IsMorphineActive) ||
                (_gc.Health.Medicine.IsEpinephrineActive && _gc.Health.Medicine.IsSedativeActive && _gc.Health.Medicine.SedativeAgent.ActiveDosesCount >= 2))
            {
                // Heart failure death chance
                _heartFailureDeathEvent.Check(deltaTime);
            }
        }

        #endregion

        #region Calculating base vitals drain rates

        private float GetWaterDrainRate() {
            var value = UnconsciousMode ? UnconsciousWaterDrainPerSecond : BasicWaterDrainPerSecond;

            if (!_gc.Player.IsWalking && !_gc.Player.IsStanding) {
                // Running
                value += AdditionalWaterDrainWhileRunningPerSecond;
            }

            if (_gc.Weather.Temperature >= ExtremelyHotWeatherTemperature)
                value += ExtremelyHotWeatherWaterDrainBonus;
            else if (_gc.Weather.Temperature >= HotWeatherTemperature)
                value += HotWeatherWaterDrainBonus;

            return value;
        }

        private float GetFoodDrainRate() {
            var value = UnconsciousMode ? UnconsciousFoodDrainPerSecond : BasicFoodDrainPerSecond;

            if (!_gc.Player.IsWalking && !_gc.Player.IsStanding) {
                // Running
                value += AdditionalFoodDrainWhileRunningPerSecond;
            }

            return value;
        }

        private float GetStaminaDrainRate() {
            if (UnconsciousMode)
                return 0f;

            var value = 0f;
            var fatigueBonus = 0f;
            var weatherBonus = 0f;
            var clothesBonus = _clothesSideEffects.StaminaBonus;

            if (_gc.Weather.Temperature >= ExtremelyHotWeatherTemperature)
                weatherBonus = ExtremelyHotWeatherStaminaDrainBonus;
            else if (_gc.Weather.Temperature >= HotWeatherTemperature)
                weatherBonus = HotWeatherStaminaDrainBonus;

            if (Status != null && !Status.IsEmpty) {
                fatigueBonus = Status.FatiguePercentage / FatigueEffectToStaminaDivider;
            }

            if (_gc.Player.IsSwimming)
            {
                value = StaminaRegainRatePerSecond / 2f - fatigueBonus - weatherBonus - clothesBonus; // Swimming stamina drain rate
            }
            else
            {
                if (_gc.Player.IsUnderWater)
                {
                    value = StaminaRegainRatePerSecond / 1.2f - fatigueBonus - weatherBonus - clothesBonus; // Underwater swimming stamina drain rate
                }
                else
                {
                    if (_gc.Player.IsStanding)
                        value = -StaminaRegainRatePerSecond + fatigueBonus + weatherBonus - clothesBonus; // Increasing stamina
                    else
                    {
                        if (_gc.Player.IsWalking)
                            value = -StaminaRegainRateWhileWalkingPerSecond + fatigueBonus + weatherBonus + clothesBonus; // Increase by little
                        else // Running
                        {
                            if (Medicine.IsEpinephrineActive)
                                value = 0f;
                            else
                                value = StaminaDecreaseRateWhileRunningPerSecond - fatigueBonus - weatherBonus - clothesBonus;
                        }
                    }
                }
            }

            return value;
        }

        #endregion

        #region Combining Stages and Injuries

        private DiseaseStage CombineStages(List<DiseaseStage> stages) {
            if (stages == null)
                return null;

            if (stages.Count == 0)
                return null;

            var stage = new DiseaseStage();

            stage.ChanceOfDeath = stages.Max(x => x.ChanceOfDeath);
            stage.BlackoutChance = stages.Max(x => x.BlackoutChance);
            stage.CoughChance = stages.Max(x => x.CoughChance);
            stage.DizzinessChance = stages.Max(x => x.DizzinessChance);
            stage.SneezeChance = stages.Max(x => x.SneezeChance);

            if (stages.Any (x => x.TargetBloodPressureBottom.HasValue))
                stage.TargetBloodPressureBottom = stages.Where(x => x.TargetBloodPressureBottom.HasValue).Max(x => x.TargetBloodPressureBottom.Value);

            if (stages.Any (x => x.TargetBloodPressureTop.HasValue))
                stage.TargetBloodPressureTop = stages.Where(x => x.TargetBloodPressureTop.HasValue).Max(x => x.TargetBloodPressureTop.Value);

            if (stages.Any (x => x.TargetBodyTemperature.HasValue))
                stage.TargetBodyTemperature = stages.Where(x => x.TargetBodyTemperature.HasValue).Max(x => x.TargetBodyTemperature.Value);

            if (stages.Any (x => x.TargetHeartRate.HasValue))
                stage.TargetHeartRate = stages.Where (x => x.TargetHeartRate.HasValue).Max(x => x.TargetHeartRate.Value);

            if (stages.Any (x => x.WaterDrainPerSecond.HasValue))
                stage.WaterDrainPerSecond = stages.Where(x => x.WaterDrainPerSecond.HasValue).Max(x => x.WaterDrainPerSecond.Value);

            if (stages.Any (x => x.FoodDrainPerSecond.HasValue))
                stage.FoodDrainPerSecond = stages.Where(x => x.FoodDrainPerSecond.HasValue).Max(x => x.FoodDrainPerSecond.Value);

            if (stages.Any (x => x.FatigueIncreasePerSecond.HasValue))
                stage.FatigueIncreasePerSecond = stages.Where (x => x.FatigueIncreasePerSecond.HasValue).Max(x => x.FatigueIncreasePerSecond.Value);

            stage.CannotEat = stages.Any(x => x.CannotEat);
            stage.CannotRun = stages.Any(x => x.CannotRun);
            stage.CannotSleep = stages.Any(x => x.CannotSleep);

            if (stages.Any(x => x.TargetVitalsTime.HasValue))
                stage.TargetVitalsTime = TimeSpan.FromSeconds(stages.Where(x => x.TargetVitalsTime.HasValue).Max(x => x.TargetVitalsTime.Value.TotalSeconds));

            stage.StageDuration = TimeSpan.FromSeconds(stages.Max(x => x.StageDuration.TotalSeconds));
            stage.Level = (DiseaseLevels) stages.Max(x => (int) x.Level);
            stage.WillTriggerAt = stages.Max(x => x.WillTriggerAt.Value);
            stage.WillEndAt = stages.Max(x => x.WillEndAt.Value);
            stage.VitalsTargetSeconds = stages.Max(x => x.VitalsTargetSeconds.Value);

            return stage;
        }

        private InjuryStage CombineInjuries(List<Tuple<InjuryStage, ActiveInjury>> injuries) {
            if (injuries == null)
                return null;

            if (injuries.Count == 0)
                return null;

            var stage = new InjuryStage ();
            var bandagedBodyParts = _gc.Body.Appliances.Where(x => x.Item.Name == InventoryController.MedicalItems.Bandage).Select(x => x.BodyPart).ToList();

            stage.BloodDrainPerSecond = injuries.Where(x => !bandagedBodyParts.Contains(x.Item2.BodyPart)).Sum(x => x.Item1.BloodDrainPerSecond);
            stage.StaminaDrainPerSecond = injuries.Sum(x => x.Item1.StaminaDrainPerSecond);
            stage.WalkSpeedDecrease = injuries.Min(x => x.Item1.WalkSpeedDecrease);

            stage.WillTriggerAt = injuries.Where(x => x.Item1.WillTriggerAt.HasValue).Min(x => x.Item1.WillTriggerAt.Value);
            stage.WillEndAt = injuries.Where(x => x.Item1.WillEndAt.HasValue).Min(x => x.Item1.WillEndAt.Value);
            stage.StageDuration = stage.WillEndAt.Value - stage.WillTriggerAt.Value;

            return stage;
        }

        #endregion

        #region Consuming Medicine, Food and Water

        public void OnConsumeItem(InventoryConsumableItemBase consumable) {
            if (consumable is InventoryMedicalItemBase)
            {
                var medItem = consumable as InventoryMedicalItemBase;

                if (medItem.MedicineKind != InventoryMedicalItemBase.MedicineKinds.Consumable)
                    return;
            }

            Status.ActiveDiseases.ForEach(disease => disease.OnConsumeItem(_gc, consumable));

            _diseaseMonitors.OnConsumeItem(consumable);
            _consumablesSideEffects.OnConsumeItem(consumable);
            _medicalAgentsMonitors.OnConsumeItem(consumable);

            // Let's check if it is actually a food
            var food = consumable as FoodItemBase;
            if (food != null)
            {
                var foodDelta = food.FoodValue / (food.IsSpoiled ? 2f : 1f);
                var waterDelta = food.WaterValue / (food.IsSpoiled ? 2f : 1f);

                Status.SetFoodLevel(Status.FoodPercentage + foodDelta);
                Status.SetWaterLevel(Status.WaterPercentage + waterDelta);
            }

            // ... or maybe water?
            var water = consumable as WaterVesselItemBase;
            if (water != null)
            {
                Status.SetWaterLevel(Status.WaterPercentage + water.WaterValuePerDose);
            }
        }

        public void OnApplianceTaken(InventoryMedicalItemBase applianceItem, BodyParts bodyPart) {
            if (applianceItem.Name == InventoryController.MedicalItems.Bandage ||
                applianceItem.Name == InventoryController.MedicalItems.Splint)
            {
                _gc.Body.Appliances.Add(new MedicalBodyAppliance{BodyPart = bodyPart, Item = applianceItem });
            }

            Status.ActiveDiseases.ForEach(disease => disease.OnApplianceTaken(_gc, applianceItem, bodyPart));
            Status.ActiveInjuries.ForEach(injury => injury.OnApplianceTaken(_gc, applianceItem, bodyPart));

            _medicalAgentsMonitors.OnApplianceTaken(applianceItem, bodyPart);

            if (applianceItem.Name == InventoryController.MedicalItems.DoripenemSyringe ||
                applianceItem.Name == InventoryController.MedicalItems.AntiVenomSyringe ||
                applianceItem.Name == InventoryController.MedicalItems.AtropineSyringe ||
                applianceItem.Name == InventoryController.MedicalItems.EpinephrineSyringe ||
                applianceItem.Name == InventoryController.MedicalItems.MorphineSyringe ||
                applianceItem.Name == InventoryController.MedicalItems.Plasma ||
                applianceItem.Name == InventoryController.MedicalItems.SuctionPump ||
                applianceItem.Name == InventoryController.MedicalItems.BioactiveHydrogel)
                Events.NotifyAll(l => l.InjectionApplied(applianceItem));
        }

        #endregion

    }
}