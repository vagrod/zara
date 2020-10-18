using System;
using System.Collections.Generic;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;

namespace ZaraEngine {

    public interface IZaraEventsListener {

        void InventoryOverload();
        void IntenseRunningTriggeredOn();
        void IntenseRunningTriggeredOff();

        void DiseaseDizziness();
        void DiseaseBlackout();
        void LowBodyTemperatureDizziness();
        void LowBodyTemperatureBlackout();
        void LowBloodLevelDizziness();
        void LowBloodLevelBlackout();
        void OverdoseEffect();
        void FatigueDizziness();
        void FatigueBlackout();
        void ExtremeFatigueSleepTrigger();
        void SedativeSleepTrigger();
        void StartDrowning();

        void DeathByDrowning();
        void DeathFromDisease();
        void DeathFromBadVitals();
        void DeathByOverdose();
        void DeathByHeartFailure();
        void DeathByBloodLoss();
        void DeathByDehydration();
        void DeathByStarvation();

        void Sneeze();
        void Cough(HealthEngine.HealthController.CoughLevels level);
        void Drink();

        void HighBloodPressureTriggeredOn();
        void HighBloodPressureTriggeredOff();

        void DiseaseTreatmentStarted(DiseaseDefinitionBase disease);
        void DiseaseHealingContinued(DiseaseDefinitionBase disease);
        void DiseaseStartProgressing(DiseaseDefinitionBase disease);
        void DiseaseTriggered(DiseaseDefinitionBase disease, ActiveInjury linkedInjury, DateTime diseaseStartTime);
        void DiseaseReActivated(DiseaseDefinitionBase disease);
        void DiseaseHealed(DiseaseDefinitionBase disease);

        void InjuryHealed(InjuryBase injury);
        void InjectionApplied(InventoryMedicalItemBase appliance);

        void MovementSpeedChange(float? newRunSpeed,float? newWalkSpeed, float? newCrouchSpeed);
        void ApplyMovementSpeedDelta(float? newRunSpeedDelta,float? newWalkSpeedDelta, float? newCrouchSpeedDelta);
        void ReportLimpingState(bool isLimping);
    }

    public static class Events {

        private static Dictionary<Guid, IZaraEventsListener> _listeners;

        public static Guid Subscribe(IZaraEventsListener listener){
            if(listener == null)
                throw new ArgumentException("listener");

            if(_listeners == null)
                _listeners = new Dictionary<Guid, IZaraEventsListener>();

            var g = Guid.NewGuid();

            _listeners.Add(g, listener);

            return g;
        }

        public static void Unsubscribe(Guid listener){
            if(_listeners == null)
                return;

            _listeners.TryGetValue(listener, out var l);

            if(l != null){
                _listeners.Remove(listener);
            }
        }

        public static void NotifyAll(Action<IZaraEventsListener> @event){
            if(_listeners == null)
                return;

            foreach(KeyValuePair<Guid, IZaraEventsListener> listener in _listeners)
            {
                // Let all listeners receive the message
                try
                {
                    @event(listener.Value);
                } catch (Exception ex){
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

    }

    

}
