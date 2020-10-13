using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageTreatmentNode
    {

        IStageTreatmentConsumableAction Treatment { get; }

    }
}
