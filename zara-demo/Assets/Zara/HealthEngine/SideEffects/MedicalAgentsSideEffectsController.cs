using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;
using ZaraEngine.Player;
using ZaraEngine.StateManaging;

namespace ZaraEngine.HealthEngine {
    public class MedicalAgentsSideEffectsController : IAcceptsStateChange {

        private readonly IGameController _gc;

        public float BloodPressureTopBonus { get; private set; }
        public float BloodPressureBottomBonus { get; private set; }
        public float HeartRateBonus { get; private set; }
        public float BodyTemperatureBonus { get; private set; }

        public MedicalAgentsSideEffectsController(IGameController gc) {
            _gc = gc;
        }

        public void Check()
        {
            if (!_gc.Health.Medicine.IsAnyAgentActive)
            {
                BloodPressureTopBonus = 0f;
                BloodPressureBottomBonus = 0f;
                HeartRateBonus = 0f;
                BodyTemperatureBonus = 0f;

                return;
            }

            var clearBloodPressureTopBonus = 0f;
            var clearBloodPressureBottomBonus = 0f;
            var clearHeartRateBonus = 0f;
            var clearBodyTemperatureBonus = 0f;

            #region Epinephrine-group Agents Side Effects

            if (_gc.Health.Medicine.IsEpinephrineActive)
            {
                clearHeartRateBonus += 50f * (_gc.Health.Medicine.EpinephrineAgent.PercentOfActivity / 100f);
                clearBloodPressureTopBonus += 25f * (_gc.Health.Medicine.EpinephrineAgent.PercentOfActivity / 100f);
                clearBloodPressureBottomBonus += 32f * (_gc.Health.Medicine.EpinephrineAgent.PercentOfActivity / 100f);
            }

            #endregion

            #region Acetaminophen-group Agents Side Effects

            if (_gc.Health.Medicine.IsAcetaminophenActive)
            {
                clearBodyTemperatureBonus -= 3.8f * (_gc.Health.Medicine.AcetaminophenAgent.PercentOfActivity / 100f);
            }

            #endregion

            #region Morphine-group Agents Side Effects

            if (_gc.Health.Medicine.IsMorphineActive)
            {
                clearHeartRateBonus -= 12f * (_gc.Health.Medicine.MorphineAgent.PercentOfActivity / 100f);
                clearBloodPressureTopBonus -= 12f * (_gc.Health.Medicine.MorphineAgent.PercentOfActivity / 100f);
                clearBloodPressureBottomBonus -= 17f * (_gc.Health.Medicine.MorphineAgent.PercentOfActivity / 100f);
            }

            #endregion

            #region Doripenem-group Agents Side Effects

            if (_gc.Health.Medicine.IsDoripenemActive)
            {
                clearHeartRateBonus += 22f * (_gc.Health.Medicine.DoripenemAgent.PercentOfActivity / 100f);
                clearBloodPressureTopBonus += 9f * (_gc.Health.Medicine.DoripenemAgent.PercentOfActivity / 100f);
                clearBloodPressureBottomBonus -= 17f * (_gc.Health.Medicine.DoripenemAgent.PercentOfActivity / 100f);
            }

            #endregion

            BloodPressureTopBonus = clearBloodPressureTopBonus;
            BloodPressureBottomBonus = clearBloodPressureBottomBonus;
            HeartRateBonus = clearHeartRateBonus;
            BodyTemperatureBonus = clearBodyTemperatureBonus;
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new MedicalAgentsHealthEffectsSnippet
            {
                BloodPressureBottomBonus = this.BloodPressureBottomBonus,
                BloodPressureTopBonus = this.BloodPressureTopBonus,
                HeartRateBonus = this.HeartRateBonus,
                BodyTemperatureBonus = this.BodyTemperatureBonus
            };

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (MedicalAgentsHealthEffectsSnippet)savedState;

            BloodPressureBottomBonus = state.BloodPressureBottomBonus;
            BloodPressureTopBonus = state.BloodPressureTopBonus;
            HeartRateBonus = state.HeartRateBonus;
            BodyTemperatureBonus = state.BodyTemperatureBonus;
        }

        #endregion 

    }
}