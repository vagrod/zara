using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.HealthEngine.SideEffects
{
    public class ConsumablesSideEffectsController : IAcceptsStateChange
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

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new ConsumablesHealthEffectsSnippet
            {
                BloodPressureBottomBonus = this.BloodPressureBottomBonus,
                BloodPressureTopBonus = this.BloodPressureTopBonus,
                HeartRateBonus = this.HeartRateBonus
            };

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (ConsumablesHealthEffectsSnippet)savedState;

            BloodPressureBottomBonus = state.BloodPressureBottomBonus;
            BloodPressureTopBonus = state.BloodPressureTopBonus;
            HeartRateBonus = state.HeartRateBonus;
        }

        #endregion 

    }
}
