using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class HypothermiaMonitor : DiseaseMonitorBase, IAcceptsStateChange
    {

        public HypothermiaMonitor(IGameController gc) : base(gc) { }

        public override void Check()
        {

        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new HypothermiaMonitorSnippet
            {

            };

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (HypothermiaMonitorSnippet)savedState;
            
            //...
        }

        #endregion 

    }
}