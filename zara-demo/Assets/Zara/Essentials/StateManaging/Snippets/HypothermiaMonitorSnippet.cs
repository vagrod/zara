using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class HypothermiaMonitorSnippet : SnippetBase
    {

        public HypothermiaMonitorSnippet() : base() { }
        public HypothermiaMonitorSnippet(object contract) : base(contract) { }

        #region Data Fields



        #endregion 

        public override object ToContract()
        {
            var c = new HypothermiaMonitorContract
            {

            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (HypothermiaMonitorContract)o;



            ChildStates.Clear();
        }

    }
}
