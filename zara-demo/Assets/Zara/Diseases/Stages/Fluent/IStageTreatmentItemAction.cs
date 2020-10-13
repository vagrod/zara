using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine;
using ZaraEngine.Inventory;

namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageTreatmentItemAction
    {

        IStageFinish WithTreatmentAction(Func<IGameController, ObjectDescriptionBase, ActiveDisease, bool> treatmentAction);

    }
}
