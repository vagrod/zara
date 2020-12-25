namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageVitalsAndDuration : IStageVitals
    {

        IStageVitals AndMinutes(int value);

    }
}
