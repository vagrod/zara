using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class DiseaseMonitorsSnippet : SnippetBase
    {

        public DiseaseMonitorsSnippet() : base() { }
        public DiseaseMonitorsSnippet(object contract) : base(contract) { }

        public override object ToContract()
        {
            var c = new DiseaseMonitorsContract();

            c.AnginaMonitor = (AnginaMonitorContract)ChildStates["AnginaMonitor"].ToContract();
            c.FluMonitor = (FluMonitorContract)ChildStates["FluMonitor"].ToContract();
            c.HyperthermiaMonitor = (HyperthermiaMonitorContract)ChildStates["HyperthermiaMonitor"].ToContract();
            c.HypothermiaMonitor = (HypothermiaMonitorContract)ChildStates["HypothermiaMonitor"].ToContract();
            c.FoodPoisoningMonitor = (FoodPoisoningMonitorContract)ChildStates["FoodPoisoningMonitor"].ToContract();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (DiseaseMonitorsContract)o;

            ChildStates.Clear();

            ChildStates.Add("AnginaMonitor", new AnginaMonitorSnippet(c.AnginaMonitor));
            ChildStates.Add("FluMonitor", new FluMonitorSnippet(c.FluMonitor));
            ChildStates.Add("HyperthermiaMonitor", new HyperthermiaMonitorSnippet(c.HyperthermiaMonitor));
            ChildStates.Add("HypothermiaMonitor", new HypothermiaMonitorSnippet(c.HypothermiaMonitor));
            ChildStates.Add("FoodPoisoningMonitor", new FoodPoisoningMonitorSnippet(c.FoodPoisoningMonitor));
        }

    }
}
