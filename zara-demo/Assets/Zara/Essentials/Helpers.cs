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

        public static bool WillHappen(this int chance)
        {
            if (chance == 0)
                return false;

            if (chance == 100)
                return true;

            return chance >= RollDice();
        }

        public static int RollDice()
        {
            return UnityEngine.Random.Range(0, 100);
        }

    }
}
