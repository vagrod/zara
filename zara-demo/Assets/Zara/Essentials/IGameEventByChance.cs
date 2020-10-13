using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine
{
    public interface IGameEventByChance : IGameEvent
    {

        bool Check(int chanceOfHappening, float deltaTime);

    }
}
