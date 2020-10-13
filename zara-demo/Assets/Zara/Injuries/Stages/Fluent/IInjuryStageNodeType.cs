using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageNodeType
    {

        IInjuryStageNodeTiming OpenFracture();

        IInjuryStageNodeTiming ClosedFracture();

        IInjuryStageNodeTiming Cut();

        IInjuryStageNodeTiming BasicInjury();

    }
}
