namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageNodeType
    {

        IInjuryStageNodeTiming OpenFracture();

        IInjuryStageNodeTiming ClosedFracture();

        IInjuryStageNodeTiming Cut();

        IInjuryStageNodeTiming BasicInjury();

    }
}
