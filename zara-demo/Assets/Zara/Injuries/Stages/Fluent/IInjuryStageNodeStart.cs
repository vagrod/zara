using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageNodeStart
    {

        IInjuryDescription WithLevelOfSeriousness(DiseaseLevels level);

    }
}
