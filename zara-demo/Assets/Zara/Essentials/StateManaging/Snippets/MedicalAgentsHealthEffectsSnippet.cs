using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class MedicalAgentsHealthEffectsSnippet : SnippetBase
    {

        public MedicalAgentsHealthEffectsSnippet() : base() { }
        public MedicalAgentsHealthEffectsSnippet(object contract) : base(contract) { }

        #region Data Fields

        public float BloodPressureTopBonus { get; set; }
        public float BloodPressureBottomBonus { get; set; }
        public float HeartRateBonus { get; set; }
        public float BodyTemperatureBonus { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new MedicalAgentsHealthEffectsContract
            {
                BloodPressureTopBonus = this.BloodPressureTopBonus,
                BloodPressureBottomBonus = this.BloodPressureBottomBonus,
                HeartRateBonus = this.HeartRateBonus,
                BodyTemperatureBonus = this.BodyTemperatureBonus
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (MedicalAgentsHealthEffectsContract)o;

            BloodPressureTopBonus = c.BloodPressureTopBonus;
            BloodPressureBottomBonus = c.BloodPressureBottomBonus;
            HeartRateBonus = c.HeartRateBonus;
            BodyTemperatureBonus = c.BodyTemperatureBonus;

            ChildStates.Clear();
        }

    }
}
