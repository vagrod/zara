namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageSelfHeal
    {

        IStageVitalsNode SelfHealChance(int percent);

        IStageVitalsNode NoSelfHeal();

    }
}
