using System;
using ZaraEngine.Inventory;

namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageTreatmentItemAction
    {

        IStageFinish WithTreatmentAction(Func<IGameController, ObjectDescriptionBase, ActiveDisease, bool> treatmentAction);

    }
}
