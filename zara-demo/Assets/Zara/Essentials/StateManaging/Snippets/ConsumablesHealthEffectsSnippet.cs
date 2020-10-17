using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class ConsumablesHealthEffectsSnippet : SnippetBase
    {

        public ConsumablesHealthEffectsSnippet() : base() { }
        public ConsumablesHealthEffectsSnippet(object contract) : base(contract) { }

        #region Data Fields

        public float BloodPressureTopBonus { get; set; }
        public float BloodPressureBottomBonus { get; set; }
        public float HeartRateBonus { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ConsumablesHealthEffectsContract
            {
                BloodPressureTopBonus = this.BloodPressureTopBonus,
                BloodPressureBottomBonus = this.BloodPressureBottomBonus,
                HeartRateBonus = this.HeartRateBonus
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ConsumablesHealthEffectsContract)o;

            BloodPressureTopBonus = c.BloodPressureTopBonus;
            BloodPressureBottomBonus = c.BloodPressureBottomBonus;
            HeartRateBonus = c.HeartRateBonus;

            ChildStates.Clear();
        }

    }
}
