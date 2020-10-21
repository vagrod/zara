using ZaraEngine.Diseases;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public interface IInjuryStageNodeStart
    {

        IInjuryDescription WithLevelOfSeriousness(DiseaseLevels level);

    }
}
