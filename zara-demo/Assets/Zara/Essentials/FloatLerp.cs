using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ZaraEngine
{
    public class FloatLerp
    {
        private readonly IGameController _gc;
        private readonly Action<float> _setValueAction;
        private readonly float _checkIntervalInRealSeconds;

        private float _lerpDurationInGameSeconds;
        private DateTime _previousCheckGameTime;
        private float _lerpCounter;
        private float _lerpDuration;

        private float _currentStartValue;
        private float _currentTargetValue;

        public float Duration
        {
            get { return _lerpDurationInGameSeconds; }
        }

        public float BaselineValue
        {
            get { return _currentStartValue; }
        }

        public float TargetValue
        {
            get { return _currentTargetValue; }
        }

        private bool _isPaused;

        public void Change(float baselineValue, float targetValue, float durationInGameSeconds)
        {
            _isPaused = true;

            _currentStartValue = baselineValue;
            _currentTargetValue = targetValue;

            _lerpDurationInGameSeconds = durationInGameSeconds;

            _previousCheckGameTime = default(DateTime);
            _lerpCounter = 0;
            _lerpDuration = 0;

            _isPaused = false;
        }

        public FloatLerp(IGameController gc, Action<float> setValueAction, float lerpDurationInGameSeconds, float checkIntervalInRealSeconds)
        {
            _gc = gc;
            _setValueAction = setValueAction;
            _lerpDurationInGameSeconds = lerpDurationInGameSeconds;
            _checkIntervalInRealSeconds = checkIntervalInRealSeconds;
        }

        public FloatLerp(IGameController gc, Action<float> setValueAction, float checkIntervalInRealSeconds) : this(gc, setValueAction, 0, checkIntervalInRealSeconds)
        {
            
        }

        public void Check()
        {
            if (!_gc.WorldTime.HasValue || _isPaused || Mathf.Abs(_lerpDurationInGameSeconds) < 0.00001)
                return;

            if (_previousCheckGameTime == default(DateTime))
            {
                _previousCheckGameTime = _gc.WorldTime.Value;
            }

            if (_lerpCounter >= _checkIntervalInRealSeconds)
            {
                _lerpCounter = 0;

                if (_lerpDuration >= _lerpDurationInGameSeconds)
                {
                    _lerpDuration = 0;
                }
                else
                {
                    _lerpDuration += (float)(_gc.WorldTime.Value - _previousCheckGameTime).TotalSeconds;
                }

                _previousCheckGameTime = _gc.WorldTime.Value;
                _setValueAction(Mathf.Lerp(_currentStartValue, _currentTargetValue, _lerpDuration / _lerpDurationInGameSeconds));
            }
            else
            {
                _lerpCounter += Time.deltaTime;
            }
        }
            
    }
}
