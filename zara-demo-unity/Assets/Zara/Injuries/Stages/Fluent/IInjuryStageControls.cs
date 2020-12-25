namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageControls
    {

        IInjuryStageControls WillNotBeAbleToRun();

        IInjuryStageTreatmentNode DescreasesMoveSpeed(float amount);

        IInjuryStageTreatmentNode NoSpeedImpact();

    }
}
