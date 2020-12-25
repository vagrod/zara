namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageDrainsNode
    {

        IInjuryStageDrains Drains { get; }

        IInjuryStageControls NoDrains();

    }
}
