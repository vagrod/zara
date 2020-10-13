using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Inventory;
using UnityEngine;

namespace ZaraEngine.Diseases
{
    public class DiseaseMonitorsNode
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
                new SunstrokeMonitor(gc),
                new FoodPoisoningMonitor(gc),
            });
        }

        public void Check()
        {
            _monitors.ForEach(x => x.Check());
        }

        public void OnConsumeItem(InventoryConsumableItemBase item)
        {
            _monitors.ForEach(x => x.OnConsumeItem(item));
        }

    }
}
