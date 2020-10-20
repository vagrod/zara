using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class HyperthermiaMonitor : DiseaseMonitorBase, IAcceptsStateChange
    {

        public HyperthermiaMonitor(IGameController gc) : base(gc) { }

        public override void Check(float deltaTime)
        {

        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new HyperthermiaMonitorSnippet
            {

            };

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (HyperthermiaMonitorSnippet)savedState;

            //...
        }

        #endregion 

    }
}
