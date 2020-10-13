using UnityEngine;
using ZaraEngine;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;
using ZaraEngine.HealthEngine;

public class ZaraEventsListener : MonoBehaviour, IZaraEventsListener {

    void Start(){
        Events.Subscribe(this);
    }

    public void InventoryOverload() { }
    public void IntenseRunningTriggeredOn() { }
    public void IntenseRunningTriggeredOff() { }
    public void SwimmingOffWaterLightBreath() { }
    public void SwimmingOffWaterMediumBreath() { }
    public void SwimmingOffWaterHardBreath() { }

    public void DiseaseDizziness() { }
    public void DiseaseBlackout() { }
    public void LowBodyTemperatureDizziness() { }
    public void LowBodyTemperatureBlackout() { }
    public void LowBloodLevelDizziness() { }
    public void LowBloodLevelBlackout() { }
    public void OverdoseEffect() { }
    public void FatigueDizziness() { }
    public void FatigueBlackout() { }
    public void ExtremeFatigueSleepTrigger() { }
    public void SedativeSleepTrigger() { }
    public void StartDrowning() { }

    public void DeathByDrowning() { }
    public void DeathFromDisease() { }
    public void DeathFromBadVitals() { }
    public void DeathByOverdose() { }
    public void DeathByHeartFailure() { }
    public void DeathByBloodLoss() { }
    public void DeathByDehydration() { }
    public void DeathByStarvation() { }

    public void Sneeze() { }
    public void Cough(HealthController.CoughLevels level) { }
    public void Drink() { }

    public void HighBloodPressureTriggeredOn() { }
    public void HighBloodPressureTriggeredOff() { }

    public void FluReTriggered() { }
    public void FoodPoisoningReTriggered() { }

    public void DiseaseTreatmentStarted(DiseaseDefinitionBase disease) { }
    public void DiseaseHealingContinued(DiseaseDefinitionBase disease) { }
    public void DiseaseStartProgressing(DiseaseDefinitionBase disease) { }
    public void DiseaseHealed(DiseaseDefinitionBase disease) { }

    public void InjuryHealed(InjuryBase injury) { }
    public void InjectionApplied(InventoryMedicalItemBase appliance) { }

    public void MovementSpeedChange(float? newRunSpeed,float? newWalkSpeed, float? newCrouchSpeed) { }
    public void ApplyMovementSpeedDelta(float? newRunSpeedDelta,float? newWalkSpeedDelta, float? newCrouchSpeedDelta) { }
    public void ReportLimpingState(bool isLimping) { }

}