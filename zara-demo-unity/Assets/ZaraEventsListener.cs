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

    public void InventoryOverload(IGameController sender) { }
    public void IntenseRunningTriggeredOn(IGameController sender) { }
    public void IntenseRunningTriggeredOff(IGameController sender) { }
    public void SwimmingOffWaterLightBreath(IGameController sender) { }
    public void SwimmingOffWaterMediumBreath(IGameController sender) { }
    public void SwimmingOffWaterHardBreath(IGameController sender) { }

    public void DiseaseDizziness(IGameController sender) { }
    public void DiseaseBlackout(IGameController sender) { }
    public void LowBodyTemperatureDizziness(IGameController sender) { }
    public void LowBodyTemperatureBlackout(IGameController sender) { }
    public void LowBloodLevelDizziness(IGameController sender) { }
    public void LowBloodLevelBlackout(IGameController sender) { }
    public void OverdoseEffect(IGameController sender) { }
    public void FatigueDizziness(IGameController sender) { }
    public void FatigueBlackout(IGameController sender) { }
    public void ExtremeFatigueSleepTrigger(IGameController sender) { }
    public void SedativeSleepTrigger(IGameController sender) { }
    public void StartDrowning(IGameController sender) { }

    public void DeathByDrowning(IGameController sender) { ShowDeathScreen("drowned"); }
    public void DeathFromDisease(IGameController sender, DiseaseDefinitionBase disease) { ShowDeathScreen(disease.Name);}
    public void DeathFromBadVitals(IGameController sender) { ShowDeathScreen("bad vitals"); }
    public void DeathByOverdose(IGameController sender) {ShowDeathScreen("medicine overdose"); }
    public void DeathByHeartFailure(IGameController sender) { ShowDeathScreen("heart failure");}
    public void DeathByBloodLoss(IGameController sender) {ShowDeathScreen("blood loss"); }
    public void DeathByDehydration(IGameController sender) { ShowDeathScreen("dehydration");}
    public void DeathByStarvation(IGameController sender) { ShowDeathScreen("starvation");}
    public void HypothermiaDeath(IGameController sender) { ShowDeathScreen("hypothermia"); }
    public void HyperthermiaDeath(IGameController sender) { ShowDeathScreen("hyperthermia"); }

    public void Sneeze(IGameController sender) { }
    public void Cough(IGameController sender, HealthController.CoughLevels level) { }
    public void Drink(IGameController sender) { }

    public void HighBloodPressureTriggeredOn(IGameController sender) { }
    public void HighBloodPressureTriggeredOff(IGameController sender) { }

    public void DiseaseTriggered(IGameController sender, DiseaseDefinitionBase disease, ActiveInjury linkedInjury, DateTime diseaseStartTime) { }
    public void DiseaseReActivated(IGameController sender, DiseaseDefinitionBase disease) { }

    public void DiseaseTreatmentStarted(IGameController sender, DiseaseDefinitionBase disease) { }
    public void DiseaseHealingContinued(IGameController sender, DiseaseDefinitionBase disease) { }
    public void DiseaseStartProgressing(IGameController sender, DiseaseDefinitionBase disease) { }
    public void DiseaseHealed(IGameController sender, DiseaseDefinitionBase disease) { }

    public void InjuryHealed(IGameController sender, InjuryBase injury) { }
    public void InjectionApplied(IGameController sender, InventoryMedicalItemBase appliance) { }

    public void MovementSpeedChange(IGameController sender, float? newRunSpeed,float? newWalkSpeed, float? newCrouchSpeed) {
        var p = _gc.Player as PlayerStatus;

        if(newRunSpeed.HasValue)
            p.SetRunSpeed(newRunSpeed.Value);

        if(newWalkSpeed.HasValue)
            p.SetWalkSpeed(newWalkSpeed.Value);

        if(newCrouchSpeed.HasValue)
            p.SetCrouchSpeed(newCrouchSpeed.Value);
    }
    public void ApplyMovementSpeedDelta(IGameController sender, float? newRunSpeedDelta,float? newWalkSpeedDelta, float? newCrouchSpeedDelta) { 
        var p = _gc.Player as PlayerStatus;
        
        if(newRunSpeedDelta.HasValue)
            p.SetRunSpeed(_gc.Player.RunSpeed + newRunSpeedDelta.Value);

        if(newWalkSpeedDelta.HasValue)
            p.SetWalkSpeed(_gc.Player.WalkSpeed + newWalkSpeedDelta.Value);

        if(newCrouchSpeedDelta.HasValue)
            p.SetCrouchSpeed(_gc.Player.CrouchSpeed + newCrouchSpeedDelta.Value);
    }
    public void ReportLimpingState(IGameController sender, bool isLimping) { 
        var p = _gc.Player as PlayerStatus;

        p.SetLimping(isLimping);
    }

}