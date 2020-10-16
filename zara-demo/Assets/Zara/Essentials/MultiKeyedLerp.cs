using System;
using System.Collections.Generic;
using System.Linq;
using ZaraEngine;

namespace ZaraEngine
{
    public class MultiKeyedLerp
    {

        private List<Keyframe> _keyframes = new List<Keyframe>();
        private List<Tuple<Keyframe, Keyframe>> _segments = new List<Tuple<Keyframe, Keyframe>>();
        private Tuple<Keyframe, Keyframe> _lastSegment;

        public MultiKeyedLerp(params Keyframe[] keyframes)
        {
            _keyframes = keyframes.OrderBy(x => x.Time).ToList();

            if (_keyframes.Count < 2)
                return;

            for (var i = 1; i < _keyframes.Count; i++)
                _segments.Add(new Tuple<Keyframe, Keyframe>(_keyframes[i - 1], _keyframes[i]));
        }

        public float Evaluate(float time)
        {
            Tuple<Keyframe, Keyframe> reScanSegments(){
                return _segments.FirstOrDefault(x => time >= x.Item1.Time && time <= x.Item2.Time);
            }

            if (_lastSegment != null)
            {
                if (time < _lastSegment.Item1.Time || time > _lastSegment.Item2.Time)
                {
                    _lastSegment = reScanSegments();
                }
            } else {
                _lastSegment = reScanSegments();
            }

            if(_lastSegment == null)
                throw new Exception($"MultiKeyedLerp segment is not present for a given value of {time}");

            var p = time / (_lastSegment.Item2.Time - _lastSegment.Item1.Time);

            //($"Lerping [{_lastSegment.Item1.Value} at {_lastSegment.Item1.Time}] and [{_lastSegment.Item2.Value} at {_lastSegment.Item2.Time}] for {p} (time={time})");

            return ZaraEngine.Helpers.Lerp(_lastSegment.Item1.Value, _lastSegment.Item2.Value, p);
        }
    }

    public class Keyframe
    {

        public float Time;
        public float Value;

        public Keyframe(float time, float value)
        {
            Time = time;
            Value = value;
        }

    }

}