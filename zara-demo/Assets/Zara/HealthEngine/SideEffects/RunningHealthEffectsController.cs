using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Inventory;
using ZaraEngine.Player;
using UnityEngine;

namespace ZaraEngine.HealthEngine
{
    public class RunningHealthEffectsController
    {

        /* ------------------------------------------------------------------------------------------------------------------ *
         * ----------------------------------------------------- Running ---------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */
        private const int SecondsUntilMaxRunningDeltaReached = 320;  // Game seconds
        private const int SecondsOfRunningBeforeWheeze       = 260;  // Game seconds
        private const float MaxHeartRateBonus                = 24f;  // bpm
        private const float MaxBloodPressureBonus            = 11f;  // mmHg
        private const float MaxBodyTemperatureBonus          = 0.5f; // C

        private readonly IGameController _gc;
        private readonly HealthController _healthController;

        private readonly FixedEvent _intenseRunningOnEvent;
        private readonly FixedEvent _intenseRunningOffEvent;

        public float BloodPressureTopBonus { get; private set; }
        public float BloodPressureBottomBonus { get; private set; }
        public float HeartRateBonus { get; private set; }
        public float BodyTemperatureBonus { get; private set; }

        private float _gameSecondsInRunningState;
        private bool _isWheezeEventTriggered;

        public RunningHealthEffectsController(IGameController gc, HealthController health)
        {
            _gc = gc;
            _healthController = health;

            // Game events produced by Running Effects Controller

            _intenseRunningOnEvent  = new FixedEvent("Intence running on trigger",  ev => Events.NotifyAll(l => l.IntenseRunningTriggeredOn())) { AutoReset = true };
            _intenseRunningOffEvent = new FixedEvent("Intence running off trigger", ev => Events.NotifyAll(l => l.IntenseRunningTriggeredOff())) { AutoReset = true };
        }

        public void Update(float gameSecondsSinceLastCall)
        {
            ProcessRunningEffects(gameSecondsSinceLastCall);
        }

        private void ProcessRunningEffects(float gameSecondsSinceLastCall)
        {
            if (_healthController.UnconsciousMode)
                return;

            if (!_gc.Player.IsWalking && !_gc.Player.IsStanding && !_gc.Health.Status.CannotRun && !_gc.Player.IsUnderWater && !_gc.Player.IsSwimming && !_healthController.Status.IsLegFracture && !_healthController.IsInventoryOverload)
                _gameSecondsInRunningState += gameSecondsSinceLastCall;
            else
                _gameSecondsInRunningState -= gameSecondsSinceLastCall;

            if (_gameSecondsInRunningState < 0.00001f)
            {
                _gameSecondsInRunningState = 0;
            }

            if (_gameSecondsInRunningState >= SecondsUntilMaxRunningDeltaReached)
            {
                _gameSecondsInRunningState = SecondsUntilMaxRunningDeltaReached;
            }

            if (Math.Abs(_gameSecondsInRunningState) < 0.00001f)
                return;

            var runningPerc = _gameSecondsInRunningState / SecondsUntilMaxRunningDeltaReached;

            BloodPressureTopBonus = Mathf.Lerp(0f, MaxBloodPressureBonus, runningPerc);
            BloodPressureBottomBonus = Mathf.Lerp(0f, MaxBloodPressureBonus, runningPerc);
            HeartRateBonus = Mathf.Lerp(0f, MaxHeartRateBonus, runningPerc);
            BodyTemperatureBonus = Mathf.Lerp(0f, MaxBodyTemperatureBonus, runningPerc);

            if (_gameSecondsInRunningState > SecondsOfRunningBeforeWheeze)
            {
                if (!_isWheezeEventTriggered)
                {
                    _intenseRunningOnEvent.Invoke();
                    _isWheezeEventTriggered = true;
                }
            }
            else
            {
                if (_gameSecondsInRunningState < SecondsOfRunningBeforeWheeze)
                {
                    if (_isWheezeEventTriggered)
                    {
                        _intenseRunningOffEvent.Invoke();
                        _isWheezeEventTriggered = false;
                    }
                }
            }
        }

    }
}
