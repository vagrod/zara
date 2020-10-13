using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageDrains
    {

        IInjuryStageDrains BloodPerSecond(float value);

        IInjuryStageDrains StaminaPerSecond(float value);

        IInjuryStageDrains FatiguePerSecond(float value);

        IInjuryStageControls WillAffectControls();

        IInjuryStageTreatmentNode WillNotAffectControls();

    }
}
