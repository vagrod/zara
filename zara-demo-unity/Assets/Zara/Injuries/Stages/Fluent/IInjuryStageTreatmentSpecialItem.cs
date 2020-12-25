using System;
using ZaraEngine.Inventory;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageTreatmentSpecialItem
    {

        IInjuryStageEnd WithTreatmentAction(Func<IGameController, InventoryMedicalItemBase, BodyParts, ActiveInjury, bool> treatmentAction);

    }
}
