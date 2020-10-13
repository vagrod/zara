using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine
{

    public enum TimesOfDay{
        Morinig,
        Day,
        Evening,
        Night
    }

    public static class Helpers
    {

        private static Func<float, float, float> _randomFunc;

        public static void InitializeRandomizer(Func<float, float, float> randomRange){
            _randomFunc = randomRange;
        }

        public  static float Lerp(float firstFloat, float secondFloat, float by){
            return firstFloat * (1f - by) + secondFloat * by;
        }

        public static float Clamp(float value, float min, float max){
            return value > max ? max : (value < min ? min : value);
        }

        public static float Clamp01(float value){
            return value > 1f ? 1f : (value < 0f ? 0f : value);
        }

        public static bool WillHappen(this int chance)
        {
            if(_randomFunc == null)
                return false;

            if (chance == 0)
                return false;

            if (chance == 100)
                return true;

            return chance >= RollDice();
        }

        public static int RollDice()
        {
            if(_randomFunc == null)
                return -1;

            return (int)_randomFunc(0f, 100f);
        }

        public static float RollDice(float a, float b)
        {
            if(_randomFunc == null)
                return -1;

            return _randomFunc(a, b);
        }

        public static int RollDice(int a, int b)
        {
            if(_randomFunc == null)
                return -1;

            return (int)_randomFunc(a, b);
        }

    }
}
