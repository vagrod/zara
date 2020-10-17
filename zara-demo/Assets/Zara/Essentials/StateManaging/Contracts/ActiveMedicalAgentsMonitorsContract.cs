using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class ActiveMedicalAgentsMonitorsContract
    {

        public bool IsEpinephrineActive;
        public bool IsAntiVenomActive;
        public bool IsAtropineActive;
        public bool IsMorphineActive;
        public bool IsAntibioticActive;
        public bool IsAspirinActive;
        public bool IsAcetaminophenActive;
        public bool IsLoperamideActive;
        public bool IsOseltamivirActive;
        public bool IsSedativeActive;
        public bool IsDoripenemActive;

        public ActiveMedicalAgentContract EpinephrineMedicalAgent;
        public ActiveMedicalAgentContract AntiVenomMedicalAgent;
        public ActiveMedicalAgentContract AtropineMedicalAgent;
        public ActiveMedicalAgentContract MorphineMedicalAgent;
        public ActiveMedicalAgentContract AntibioticMedicalAgent;
        public ActiveMedicalAgentContract AspirinMedicalAgent;
        public ActiveMedicalAgentContract AcetaminophenMedicalAgent;
        public ActiveMedicalAgentContract LoperamideMedicalAgent;
        public ActiveMedicalAgentContract OseltamivirMedicalAgent;
        public ActiveMedicalAgentContract SedativeMedicalAgent;
        public ActiveMedicalAgentContract DoripenemMedicalAgent;

    }
}
