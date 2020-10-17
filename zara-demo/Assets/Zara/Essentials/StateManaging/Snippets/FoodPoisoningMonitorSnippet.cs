using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class FoodPoisoningMonitorSnippet : SnippetBase
    {

        public FoodPoisoningMonitorSnippet() : base() { }
        public FoodPoisoningMonitorSnippet(object contract) : base(contract) { }

        #region Data Fields



        #endregion 

        public override object ToContract()
        {
            var c = new FoodPoisoningMonitorContract
            {

            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (FoodPoisoningMonitorContract)o;



            ChildStates.Clear();
        }

    }
}
