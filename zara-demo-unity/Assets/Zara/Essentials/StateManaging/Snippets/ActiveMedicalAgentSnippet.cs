using System;
using System.Collections.Generic;
using System.Linq;

namespace ZaraEngine.StateManaging
{
    public class ActiveMedicalAgentSnippet : SnippetBase
    {

        public ActiveMedicalAgentSnippet() : base() { }
        public ActiveMedicalAgentSnippet(object contract) : base(contract) { }

        #region Data Fields

        public float GameMinutesAgentIsActive { get; set; }
        public List<DateTime> TimesTaken { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ActiveMedicalAgentContract
            {
                GameMinutesAgentIsActive = this.GameMinutesAgentIsActive,
                TimesTaken = this.TimesTaken.ConvertAll(x => new DateTimeContract(x)).ToArray()
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ActiveMedicalAgentContract)o;

            GameMinutesAgentIsActive = c.GameMinutesAgentIsActive;
            TimesTaken = c.TimesTaken.ToList().ConvertAll(x => x.ToDateTime());

            ChildStates.Clear();
        }

    }
}
