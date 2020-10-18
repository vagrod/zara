using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Injuries
{

    public abstract class InjuryBase: IAcceptsStateChange
    {

        public Guid Id { get; } = Guid.NewGuid();

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

        #region State Manage

        public virtual IStateSnippet GetState()
        {
            return new InjuryTreatmentSnippet();
        }

        public virtual void RestoreState(IStateSnippet state)
        {

        }

        #endregion

    }
}
