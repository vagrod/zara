namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageTreatmentNode
    {

        IInjuryStageTreatmentSpecialItem Treatment { get; }

        IInjuryStageEnd NoTreatment();

    }
}
