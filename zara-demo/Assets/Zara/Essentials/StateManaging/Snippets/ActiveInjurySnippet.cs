using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries;

namespace ZaraEngine.StateManaging
{
    public class ActiveInjurySnippet : SnippetBase
    {

        public ActiveInjurySnippet() : base() { }
        public ActiveInjurySnippet(object contract) : base(contract) { }

        #region Data Fields

        public Guid InjuryId { get; set; }
        public string InjuryType { get; set; }

        public bool IsInjuryActivated { get; set; }
        public bool IsChainInverted { get; set; }
        public DiseaseLevels? TreatedStageLevel { get; set; }
        public BodyParts BodyPart { get; set; }
        public DateTime InjuryTriggerTime { get; set; }
        public bool IsTreated { get; set; }
        public bool IsDiseaseProbabilityChecked { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ActiveInjuryContract
            {
                InjuryId = this.InjuryId,
                InjuryType = this.InjuryType,
                IsInjuryActivated = this.IsInjuryActivated,
                IsChainInverted = this.IsChainInverted,
                TreatedStageLevel = this.TreatedStageLevel.HasValue ? (int?)this.TreatedStageLevel.Value : (int?)null,
                BodyPart = (int)this.BodyPart,
                InjuryTriggerTime = this.InjuryTriggerTime,
                IsTreated = this.IsTreated,
                IsDiseaseProbabilityChecked = this.IsDiseaseProbabilityChecked
            };

            c.Treatments = (InjuryTreatmentContract)ChildStates["Treatments"].ToContract();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ActiveInjuryContract)o;

            InjuryId = c.InjuryId;
            InjuryType = c.InjuryType;
            IsInjuryActivated = c.IsInjuryActivated;
            IsChainInverted = c.IsChainInverted;
            TreatedStageLevel = c.TreatedStageLevel.HasValue ? (DiseaseLevels?)c.TreatedStageLevel.Value : (DiseaseLevels?)null;
            BodyPart = (BodyParts)c.BodyPart;
            InjuryTriggerTime = c.InjuryTriggerTime;
            IsTreated = c.IsTreated;
            IsDiseaseProbabilityChecked = c.IsDiseaseProbabilityChecked;

            ChildStates.Clear();

            ChildStates.Add("Treatments", new InjuryTreatmentSnippet(c.Treatments));
        }

    }
}
