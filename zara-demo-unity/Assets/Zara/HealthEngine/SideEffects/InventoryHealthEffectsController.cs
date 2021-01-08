using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.HealthEngine
{
    public class InventoryHealthEffectsController : IAcceptsStateChange
    {

        private const float MaxFatigueDrainBonus = 0.01f; // percents per game second
        private const float MaxStaminaDrainBonus = 0.02f; // percents per game second
        
        private readonly IGameController _gc;

        private readonly FixedEvent _freezedByInventoryOverloadEvent;

        public float PlayerWalkSpeedBonus { get; private set; }
        public float PlayerRunSpeedBonus { get; private set; }
        public float PlayerCrouchSpeedBonus { get; private set; }
        public float FatigueDrainBonus { get; private set; } // percents per game second
        public float StaminaDrainBonus { get; private set; } // percents per game second

        public bool IsFreezed { get; private set; }

        public InventoryHealthEffectsController(IGameController gc)
        {
            _gc = gc;

            // Game events produced by the Inventory Effects Controller

            _freezedByInventoryOverloadEvent = new FixedEvent("Player freezed by inventory overload", ev => Events.NotifyAll(l => l.InventoryOverload(gc))) { AutoReset = true };
        }

        public void Update(float deltaTime)
        {
            ProcessInventoryEffects(deltaTime);
        }

        private void ProcessInventoryEffects(float deltaTime)
        {
            if (_gc.Inventory.RoughWeight >= InventoryController.MaximumInventoryWeight)
            {
                if (!IsFreezed)
                {
                    IsFreezed = true;
                    _freezedByInventoryOverloadEvent.Invoke(deltaTime);
                }

                return;
            }

            IsFreezed = false;

            var invPerc = _gc.Inventory.RoughWeight / InventoryController.MaximumInventoryWeight;

            PlayerRunSpeedBonus = -Helpers.Lerp(0f, _gc.Player.RunSpeed / 2f, invPerc);
            PlayerWalkSpeedBonus = -Helpers.Lerp(0f, _gc.Player.WalkSpeed / 2f, invPerc);
            PlayerCrouchSpeedBonus = -Helpers.Lerp(0f, _gc.Player.CrouchSpeed / 3f, invPerc);
            FatigueDrainBonus = Helpers.Lerp(0f, MaxFatigueDrainBonus, invPerc);
            StaminaDrainBonus = Helpers.Lerp(0f, MaxStaminaDrainBonus, invPerc);
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new InventoryHealthEffectsSnippet
            {
                PlayerRunSpeedBonus = this.PlayerRunSpeedBonus,
                PlayerWalkSpeedBonus = this.PlayerWalkSpeedBonus,
                PlayerCrouchSpeedBonus = this.PlayerCrouchSpeedBonus,
                IsFreezed = this.IsFreezed
            };

            state.ChildStates.Add("FreezedByInventoryOverloadEvent", _freezedByInventoryOverloadEvent.GetState());

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (InventoryHealthEffectsSnippet)savedState;

            PlayerRunSpeedBonus = state.PlayerRunSpeedBonus;
            PlayerWalkSpeedBonus = state.PlayerWalkSpeedBonus;
            PlayerCrouchSpeedBonus = state.PlayerCrouchSpeedBonus;
            IsFreezed = state.IsFreezed;

            _freezedByInventoryOverloadEvent.RestoreState(state.ChildStates["FreezedByInventoryOverloadEvent"]);
        }

        #endregion 

    }
}