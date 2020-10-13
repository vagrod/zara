using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryDescription
    {

        IInjuryStageNodeType WithDescription(string description);

        IInjuryStageNodeType NoDescription();

    }
}
