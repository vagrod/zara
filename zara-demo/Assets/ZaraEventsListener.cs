using System;
using UnityEngine;
using UnityEngine.UI;
using ZaraEngine;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;
using ZaraEngine.HealthEngine;

public class ZaraEventsListener : MonoBehaviour, IZaraEventsListener {

    private GameController _gc;

    void Start(){
        Events.Subscribe(this);

        _gc = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public Canvas DeathScreen;

    private void ShowDeathScreen(string causeOfDeath){
        DeathScreen.gameObject.transform.Find("Text").GetComponent<Text>().text = $"Character is dead\nCause of death: {causeOfDeath}";
        DeathScreen.gameObject.SetActive(true);
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

    public void DeathByDrowning() { ShowDeathScreen("drowned"); }
    public void DeathFromDisease(DiseaseDefinitionBase disease) { ShowDeathScreen(disease.Name);}
    public void DeathFromBadVitals() { ShowDeathScreen("bad vitals"); }
    public void DeathByOverdose() {ShowDeathScreen("medicine overdose"); }
    public void DeathByHeartFailure() { ShowDeathScreen("heart failure");}
    public void DeathByBloodLoss() {ShowDeathScreen("blood loss"); }
    public void DeathByDehydration() { ShowDeathScreen("dehydration");}
    public void DeathByStarvation() { ShowDeathScreen("starvation");}
    public void HypothermiaDeath() { ShowDeathScreen("hypothermia"); }
    public void HyperthermiaDeath() { ShowDeathScreen("hyperthermia"); }

    public void Sneeze() { }
    public void Cough(HealthController.CoughLevels level) { }
    public void Drink() { }

    public void HighBloodPressureTriggeredOn() { }
    public void HighBloodPressureTriggeredOff() { }

    public void DiseaseTriggered(DiseaseDefinitionBase disease, ActiveInjury linkedInjury, DateTime diseaseStartTime) { }
    public void DiseaseReActivated(DiseaseDefinitionBase disease) { }

    public void DiseaseTreatmentStarted(DiseaseDefinitionBase disease) { }
    public void DiseaseHealingContinued(DiseaseDefinitionBase disease) { }
    public void DiseaseStartProgressing(DiseaseDefinitionBase disease) { }
    public void DiseaseHealed(DiseaseDefinitionBase disease) { }

    public void InjuryHealed(InjuryBase injury) { }
    public void InjectionApplied(InventoryMedicalItemBase appliance) { }

    public void MovementSpeedChange(float? newRunSpeed,float? newWalkSpeed, float? newCrouchSpeed) {
        var p = _gc.Player as PlayerStatus;

        if(newRunSpeed.HasValue)
            p.SetRunSpeed(newRunSpeed.Value);

        if(newWalkSpeed.HasValue)
            p.SetWalkSpeed(newWalkSpeed.Value);

        if(newCrouchSpeed.HasValue)
            p.SetCrouchSpeed(newCrouchSpeed.Value);
    }
    public void ApplyMovementSpeedDelta(float? newRunSpeedDelta,float? newWalkSpeedDelta, float? newCrouchSpeedDelta) { 
        var p = _gc.Player as PlayerStatus;
        
        if(newRunSpeedDelta.HasValue)
            p.SetRunSpeed(_gc.Player.RunSpeed + newRunSpeedDelta.Value);

        if(newWalkSpeedDelta.HasValue)
            p.SetWalkSpeed(_gc.Player.WalkSpeed + newWalkSpeedDelta.Value);

        if(newCrouchSpeedDelta.HasValue)
            p.SetCrouchSpeed(_gc.Player.CrouchSpeed + newCrouchSpeedDelta.Value);
    }
    public void ReportLimpingState(bool isLimping) { 
        var p = _gc.Player as PlayerStatus;

        p.SetLimping(isLimping);
    }

}