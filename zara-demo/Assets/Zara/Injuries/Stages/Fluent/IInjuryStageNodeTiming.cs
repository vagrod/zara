using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageNodeTiming
    {

        IInjuryStageNodeHealthImpact WillLastForHours(int value);

        IInjuryStageNodeHealthImpact WillLastForever();

    }
}
