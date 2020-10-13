using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Inventory;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageTreatmentSpecialItem
    {

        IInjuryStageEnd WithTreatmentAction(Func<IGameController, InventoryMedicalItemBase, BodyParts, ActiveInjury, bool> treatmentAction);

    }
}
