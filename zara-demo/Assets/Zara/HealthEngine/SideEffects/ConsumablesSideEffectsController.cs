using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Inventory;

namespace ZaraEngine.HealthEngine.SideEffects
{
    public class ConsumablesSideEffectsController
    {

        private readonly IGameController _gc;

        public float BloodPressureTopBonus { get; private set; }
        public float BloodPressureBottomBonus { get; private set; }
        public float HeartRateBonus { get; private set; }

        public ConsumablesSideEffectsController(IGameController gc)
        {
            _gc = gc;
        }

        public void OnConsumeItem(InventoryConsumableItemBase pill)
        {

        }

        public void Check()
        {

        }

    }
}
