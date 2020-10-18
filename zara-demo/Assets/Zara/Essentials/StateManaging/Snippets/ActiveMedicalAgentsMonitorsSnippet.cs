namespace ZaraEngine.StateManaging
{
    public class ActiveMedicalAgentsMonitorsSnippet : SnippetBase
    {

        public ActiveMedicalAgentsMonitorsSnippet() : base() { }
        public ActiveMedicalAgentsMonitorsSnippet(object contract) : base(contract) { }

        #region Data Fields

        public bool IsEpinephrineActive { get; set; }
        public bool IsAntiVenomActive { get; set; }
        public bool IsAtropineActive { get; set; }
        public bool IsMorphineActive { get; set; }
        public bool IsAntibioticActive { get; set; }
        public bool IsAspirinActive { get; set; }
        public bool IsAcetaminophenActive { get; set; }
        public bool IsLoperamideActive { get; set; }
        public bool IsOseltamivirActive { get; set; }
        public bool IsSedativeActive { get; set; }
        public bool IsDoripenemActive { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ActiveMedicalAgentsMonitorsContract
            {
                IsEpinephrineActive = this.IsEpinephrineActive,
                IsAntiVenomActive = this.IsAntiVenomActive,
                IsAtropineActive = this.IsAtropineActive,
                IsMorphineActive = this.IsMorphineActive,
                IsAntibioticActive = this.IsAntibioticActive,
                IsAspirinActive = this.IsAspirinActive,
                IsAcetaminophenActive = this.IsAcetaminophenActive,
                IsLoperamideActive = this.IsLoperamideActive,
                IsOseltamivirActive = this.IsOseltamivirActive,
                IsSedativeActive = this.IsSedativeActive,
                IsDoripenemActive = this.IsDoripenemActive,
            };

            c.EpinephrineMedicalAgent = (ActiveMedicalAgentContract)ChildStates["EpinephrineMedicalAgent"].ToContract();
            c.AntiVenomMedicalAgent = (ActiveMedicalAgentContract)ChildStates["AntiVenomMedicalAgent"].ToContract();
            c.AtropineMedicalAgent = (ActiveMedicalAgentContract)ChildStates["AtropineMedicalAgent"].ToContract();
            c.MorphineMedicalAgent = (ActiveMedicalAgentContract)ChildStates["MorphineMedicalAgent"].ToContract();
            c.AntibioticMedicalAgent = (ActiveMedicalAgentContract)ChildStates["AntibioticMedicalAgent"].ToContract();
            c.AspirinMedicalAgent = (ActiveMedicalAgentContract)ChildStates["AspirinMedicalAgent"].ToContract();
            c.AcetaminophenMedicalAgent = (ActiveMedicalAgentContract)ChildStates["AcetaminophenMedicalAgent"].ToContract();
            c.LoperamideMedicalAgent = (ActiveMedicalAgentContract)ChildStates["LoperamideMedicalAgent"].ToContract();
            c.OseltamivirMedicalAgent = (ActiveMedicalAgentContract)ChildStates["OseltamivirMedicalAgent"].ToContract();
            c.SedativeMedicalAgent = (ActiveMedicalAgentContract)ChildStates["SedativeMedicalAgent"].ToContract();
            c.DoripenemMedicalAgent = (ActiveMedicalAgentContract)ChildStates["DoripenemMedicalAgent"].ToContract();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ActiveMedicalAgentsMonitorsContract)o;

            IsEpinephrineActive = c.IsEpinephrineActive;
            IsAntiVenomActive = c.IsAntiVenomActive;
            IsAtropineActive = c.IsAtropineActive;
            IsMorphineActive = c.IsMorphineActive;
            IsAntibioticActive = c.IsAntibioticActive;
            IsAspirinActive = c.IsAspirinActive;
            IsAcetaminophenActive = c.IsAcetaminophenActive;
            IsLoperamideActive = c.IsLoperamideActive;
            IsOseltamivirActive = c.IsOseltamivirActive;
            IsSedativeActive = c.IsSedativeActive;
            IsDoripenemActive = c.IsDoripenemActive;

            ChildStates.Clear();

            ChildStates.Add("EpinephrineMedicalAgent", new ActiveMedicalAgentSnippet(c.EpinephrineMedicalAgent));
            ChildStates.Add("AntiVenomMedicalAgent", new ActiveMedicalAgentSnippet(c.AntiVenomMedicalAgent));
            ChildStates.Add("AtropineMedicalAgent", new ActiveMedicalAgentSnippet(c.AtropineMedicalAgent));
            ChildStates.Add("MorphineMedicalAgent", new ActiveMedicalAgentSnippet(c.MorphineMedicalAgent));
            ChildStates.Add("AntibioticMedicalAgent", new ActiveMedicalAgentSnippet(c.AntibioticMedicalAgent));
            ChildStates.Add("AspirinMedicalAgent", new ActiveMedicalAgentSnippet(c.AspirinMedicalAgent));
            ChildStates.Add("AcetaminophenMedicalAgent", new ActiveMedicalAgentSnippet(c.AcetaminophenMedicalAgent));
            ChildStates.Add("LoperamideMedicalAgent", new ActiveMedicalAgentSnippet(c.LoperamideMedicalAgent));
            ChildStates.Add("OseltamivirMedicalAgent", new ActiveMedicalAgentSnippet(c.OseltamivirMedicalAgent));
            ChildStates.Add("SedativeMedicalAgent", new ActiveMedicalAgentSnippet(c.SedativeMedicalAgent));
            ChildStates.Add("DoripenemMedicalAgent", new ActiveMedicalAgentSnippet(c.DoripenemMedicalAgent));
        }

    }
}
