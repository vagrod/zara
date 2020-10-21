using System.Collections.Generic;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class DiseaseMonitorsNode : IAcceptsStateChange
    {

        private readonly List<DiseaseMonitorBase> _monitors = new List<DiseaseMonitorBase>();

        public DiseaseMonitorsNode(IGameController gc)
        {
            _monitors.AddRange(new DiseaseMonitorBase[]
            {
                new AnginaMonitor(gc),
                new FluMonitor(gc),
                new HyperthermiaMonitor(gc),
                new HypothermiaMonitor(gc),
                new FoodPoisoningMonitor(gc),
            });
        }

        public void Check(float deltaTime)
        {
            _monitors.ForEach(x => x.Check(deltaTime));
        }

        public void OnConsumeItem(InventoryConsumableItemBase item)
        {
            _monitors.ForEach(x => x.OnConsumeItem(item));
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new DiseaseMonitorsSnippet();

            state.ChildStates.Add("AnginaMonitor", (_monitors[0] as AnginaMonitor).GetState());
            state.ChildStates.Add("FluMonitor", (_monitors[1] as FluMonitor).GetState());
            state.ChildStates.Add("HyperthermiaMonitor", (_monitors[2] as HyperthermiaMonitor).GetState());
            state.ChildStates.Add("HypothermiaMonitor", (_monitors[3] as HypothermiaMonitor).GetState());
            state.ChildStates.Add("FoodPoisoningMonitor", (_monitors[4] as FoodPoisoningMonitor).GetState());

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (DiseaseMonitorsSnippet)savedState;

            (_monitors[0] as AnginaMonitor).RestoreState(state.ChildStates["AnginaMonitor"]);
            (_monitors[1] as FluMonitor).RestoreState(state.ChildStates["FluMonitor"]);
            (_monitors[2] as HyperthermiaMonitor).RestoreState(state.ChildStates["HyperthermiaMonitor"]);
            (_monitors[3] as HypothermiaMonitor).RestoreState(state.ChildStates["HypothermiaMonitor"]);
            (_monitors[4] as FoodPoisoningMonitor).RestoreState(state.ChildStates["FoodPoisoningMonitor"]);
        }

        #endregion 

    }
}
