using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class HyperthermiaMonitorSnippet : SnippetBase
    {

        public HyperthermiaMonitorSnippet() : base() { }
        public HyperthermiaMonitorSnippet(object contract) : base(contract) { }

        #region Data Fields



        #endregion 

        public override object ToContract()
        {
            var c = new HyperthermiaMonitorContract
            {

            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (HyperthermiaMonitorContract)o;



            ChildStates.Clear();
        }

    }
}
