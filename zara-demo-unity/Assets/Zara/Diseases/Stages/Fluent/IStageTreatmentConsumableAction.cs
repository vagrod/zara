using System;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;

namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageTreatmentConsumableAction
    {

        IStageTreatmentItems WithConsumable(Func<IGameController, InventoryConsumableItemBase, ActiveDisease, bool> treatmentAction);

        IStageTreatmentItems WithAppliance(Func<IGameController, ApplianceInfo, ActiveDisease, bool> treatmentAction);

        IStageTreatmentItems WithoutConsumable();

    }

    public class ApplianceInfo
    {
        public Player.BodyParts BodyPart { get; set; }
        public InventoryMedicalItemBase Appliance { get; set; }
    }
}
