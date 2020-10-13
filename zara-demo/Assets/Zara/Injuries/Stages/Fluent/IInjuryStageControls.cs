using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageControls
    {

        IInjuryStageControls WillNotBeAbleToRun();

        IInjuryStageTreatmentNode DescreasesMoveSpeed(float amount);

        IInjuryStageTreatmentNode NoSpeedImpact();

    }
}
