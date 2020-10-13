using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages;

namespace ZaraEngine.Injuries
{

    public abstract class InjuryBase
    {

        protected InjuryBase()
        {
            Stages = new List<InjuryStage>();
        }

        public string Name { get; protected set; }

        public ICollection<InjuryStage> Stages { get; protected set; }

        public void SwapChain(List<InjuryStage> chainCopy)
        {
            Stages = chainCopy;
        }
    }
}
