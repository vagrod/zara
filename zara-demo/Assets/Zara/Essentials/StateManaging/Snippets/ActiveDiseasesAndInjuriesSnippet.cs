using System.Linq;
using System.Collections.Generic;

namespace ZaraEngine.StateManaging
{
    public class ActiveDiseasesAndInjuriesSnippet : SnippetBase
    {

        public ActiveDiseasesAndInjuriesSnippet() : base() { }
        public ActiveDiseasesAndInjuriesSnippet(object contract) : base(contract) { }

        #region Data Fields

        public List<ActiveDiseaseSnippet> ActiveDiseases { get; set; } = new List<ActiveDiseaseSnippet>();
        public List<ActiveInjurySnippet> ActiveInjuries { get; set; } = new List<ActiveInjurySnippet>();

        #endregion 

        public override object ToContract()
        {
            var c = new ActiveDiseasesAndInjuriesContract();

            c.ActiveDiseases = ActiveDiseases.ConvertAll(x => (ActiveDiseaseContract)x.ToContract()).ToArray();
            c.ActiveInjuries = ActiveInjuries.ConvertAll(x => (ActiveInjuryContract)x.ToContract()).ToArray();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ActiveDiseasesAndInjuriesContract)o;

            ActiveDiseases = c.ActiveDiseases.ToList().ConvertAll(x => new ActiveDiseaseSnippet(x));
            ActiveInjuries = c.ActiveInjuries.ToList().ConvertAll(x => new ActiveInjurySnippet(x));

            ChildStates.Clear();
        }

    }
}
