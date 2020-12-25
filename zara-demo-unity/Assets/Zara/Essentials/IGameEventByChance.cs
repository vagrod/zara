namespace ZaraEngine
{
    public interface IGameEventByChance : IGameEvent
    {

        bool Check(int chanceOfHappening, float deltaTime);

    }
}
