using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageDurationTiming
    {

        IStageVitalsAndDuration AndLastForHours(int value);

        IStageVitalsAndDuration AndLastForMinutes(int value);

        IStageVitalsAndDuration AndLastUntilEnd();

    }
}
