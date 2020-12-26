using System;
using System.Collections.Generic;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;

namespace ZaraEngine {

    public interface IZaraEventsListener {

        void InventoryOverload(IGameController sender);
        void IntenseRunningTriggeredOn(IGameController sender);
        void IntenseRunningTriggeredOff(IGameController sender);

        void DiseaseDizziness(IGameController sender);
        void DiseaseBlackout(IGameController sender);
        void LowBodyTemperatureDizziness(IGameController sender);
        void LowBodyTemperatureBlackout(IGameController sender);
        void LowBloodLevelDizziness(IGameController sender);
        void LowBloodLevelBlackout(IGameController sender);
        void OverdoseEffect(IGameController sender);
        void FatigueDizziness(IGameController sender);
        void FatigueBlackout(IGameController sender);
        void ExtremeFatigueSleepTrigger(IGameController sender);
        void SedativeSleepTrigger(IGameController sender);
        void StartDrowning(IGameController sender);

        void DeathByDrowning(IGameController sender);
        void DeathFromDisease(IGameController sender, DiseaseDefinitionBase disease);
        void DeathFromBadVitals(IGameController sender);
        void DeathByOverdose(IGameController sender);
        void DeathByHeartFailure(IGameController sender);
        void DeathByBloodLoss(IGameController sender);
        void DeathByDehydration(IGameController sender);
        void DeathByStarvation(IGameController sender);
        void HypothermiaDeath(IGameController sender);
        void HyperthermiaDeath(IGameController sender);

        void Sneeze(IGameController sender);
        void Cough(IGameController sender, HealthEngine.HealthController.CoughLevels level);
        void Drink(IGameController sender);

        void HighBloodPressureTriggeredOn(IGameController sender);
        void HighBloodPressureTriggeredOff(IGameController sender);

        void DiseaseTreatmentStarted(IGameController sender, DiseaseDefinitionBase disease);
        void DiseaseHealingContinued(IGameController sender, DiseaseDefinitionBase disease);
        void DiseaseStartProgressing(IGameController sender, DiseaseDefinitionBase disease);
        void DiseaseTriggered(IGameController sender, DiseaseDefinitionBase disease, ActiveInjury linkedInjury, DateTime diseaseStartTime);
        void DiseaseReActivated(IGameController sender, DiseaseDefinitionBase disease);
        void DiseaseHealed(IGameController sender, DiseaseDefinitionBase disease);

        void InjuryHealed(IGameController sender, InjuryBase injury);
        void InjectionApplied(IGameController sender, InventoryMedicalItemBase appliance);

        void MovementSpeedChange(IGameController sender, float? newRunSpeed,float? newWalkSpeed, float? newCrouchSpeed);
        void ApplyMovementSpeedDelta(IGameController sender, float? newRunSpeedDelta,float? newWalkSpeedDelta, float? newCrouchSpeedDelta);
        void ReportLimpingState(IGameController sender, bool isLimping);
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
