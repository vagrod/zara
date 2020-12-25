namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageVitals
    {

        IStageVitals WithTargetBodyTemperature(float value);

        IStageVitals WithTargetBloodPressure(float first, float second);

        IStageVitals WithTargetHeartRate(float value);

        IStageDurationTiming WillReachTargetsInHours(int value);

        IStageAdditional NoAdditionalEffects();

        IStageAdditional AdditionalEffects { get; }

    }

    public interface IStageVitalsNode
    {

        IStageVitals Vitals { get; }

    }
}
