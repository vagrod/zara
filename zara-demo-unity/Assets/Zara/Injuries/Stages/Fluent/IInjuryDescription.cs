namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryDescription
    {

        IInjuryStageNodeType WithDescription(string description);

        IInjuryStageNodeType NoDescription();

    }
}
