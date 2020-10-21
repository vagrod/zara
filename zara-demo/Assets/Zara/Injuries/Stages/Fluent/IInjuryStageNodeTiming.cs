namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageNodeTiming
    {

        IInjuryStageNodeHealthImpact WillLastForHours(int value);

        IInjuryStageNodeHealthImpact WillLastForever();

    }
}
