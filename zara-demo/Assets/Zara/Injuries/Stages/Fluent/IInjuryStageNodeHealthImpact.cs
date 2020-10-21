using ZaraEngine.Diseases;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageNodeHealthImpact
    {

        IInjuryStageDrainsNode TriggersDisease<T>(int probability) where T : DiseaseDefinitionBase;

        IInjuryStageDrainsNode WillSelfHealInHours(int value);

        IInjuryStageDrainsNode NoSelfHeal();

    }
}
