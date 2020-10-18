using System.Linq;

namespace ZaraEngine.StateManaging
{
    public class ActiveDiseasesAndInjuriesSnippet : SnippetBase
    {

        public ActiveDiseasesAndInjuriesSnippet() : base() { }
        public ActiveDiseasesAndInjuriesSnippet(object contract) : base(contract) { }

        #region Data Fields

        public ActiveDiseaseSnippet[] ActiveDiseases { get; set; }
        public ActiveInjurySnippet[] ActiveInjuries { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ActiveDiseasesAndInjuriesContract();

            c.ActiveDiseases = ActiveDiseases.ToList().ConvertAll(x => (ActiveDiseaseContract)x.ToContract()).ToArray();
            c.ActiveInjuries = ActiveInjuries.ToList().ConvertAll(x => (ActiveInjuryContract)x.ToContract()).ToArray();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ActiveDiseasesAndInjuriesContract)o;

            ActiveDiseases = c.ActiveDiseases.ToList().ConvertAll(x => new ActiveDiseaseSnippet(x)).ToArray();
            ActiveInjuries = c.ActiveInjuries.ToList().ConvertAll(x => new ActiveInjurySnippet(x)).ToArray();

            ChildStates.Clear();
        }

    }
}
