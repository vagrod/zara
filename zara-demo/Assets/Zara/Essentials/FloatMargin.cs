using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine
{
    public class FloatMargin
    {

        public FloatMargin(float minValue, float maxValue)
        {
            Minimum = minValue;
            Maximum = maxValue;
        }

        public float Minimum { get; set; }

        public float Maximum { get; set; }

        public float GetRandom()
        {
            return Helpers.RollDice(Minimum, Maximum);
        }

    }
}
