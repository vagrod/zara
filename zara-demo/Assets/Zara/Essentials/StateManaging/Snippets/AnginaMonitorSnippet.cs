using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class AnginaMonitorSnippet : SnippetBase
    {

        public AnginaMonitorSnippet() : base() { }
        public AnginaMonitorSnippet(object contract) : base(contract) { }

        #region Data Fields

        public DateTime? NextCheckTime { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new AnginaMonitorContract
            {
                NextCheckTime = this.NextCheckTime
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (AnginaMonitorContract)o;

            NextCheckTime = c.NextCheckTime;

            ChildStates.Clear();
        }

    }
}
