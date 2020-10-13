using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Inventory;
using UnityEngine;

namespace ZaraEngine.HealthEngine
{
    public class InventoryHealthEffectsController
    {

        private readonly IGameController _gc;

        private readonly FixedEvent _freezedByInventoryOverloadEvent;

        public float PlayerWalkSpeedBonus { get; private set; }
        public float PlayerRunSpeedBonus { get; private set; }
        public float PlayerCrouchSpeedBonus { get; private set; }

        public bool IsFreezed { get; private set; }

        public InventoryHealthEffectsController(IGameController gc)
        {
            _gc = gc;

            // Game events produced by the Inventory Effects Controller

            _freezedByInventoryOverloadEvent = new FixedEvent("Player freezed by inventory overload", ev => Events.NotifyAll(l => l.InventoryOverload())) { AutoReset = true };
        }

        public void Update()
        {
            ProcessInventoryEffects();
        }

        private void ProcessInventoryEffects()
        {
            if (_gc.Inventory.RoughWeight >= InventoryController.MaximumInventoryWeight)
            {
                if (!IsFreezed)
                {
                    IsFreezed = true;
                    _freezedByInventoryOverloadEvent.Invoke();
                }

                return;
            }

            IsFreezed = false;

            var invPerc = _gc.Inventory.RoughWeight / InventoryController.MaximumInventoryWeight;

            PlayerRunSpeedBonus = -Mathf.Lerp(0f, _gc.Player.RunSpeed / 2f, invPerc);
            PlayerWalkSpeedBonus = -Mathf.Lerp(0f, _gc.Player.WalkSpeed / 2f, invPerc);
            PlayerCrouchSpeedBonus = -Mathf.Lerp(0f, _gc.Player.CrouchSpeed / 3f, invPerc);
        }

    }
}