namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageAdditional
    {

        IStageAdditional WithLowChanceOfBlackouts();

        IStageAdditional WithMediumChanceOfBlackouts();

        IStageAdditional WithHighChanceOfBlackouts();

        IStageAdditional WithCustomChanceOfBlackouts(int chancePercent);



        IStageAdditional WithLowChanceOfDizziness();

        IStageAdditional WithMediumChanceOfDizziness();

        IStageAdditional WithHighChanceOfDizziness();

        IStageAdditional WithCustomChanceOfDizziness(int chancePercent);


        IStageAdditional WithLowChanceOfCough();

        IStageAdditional WithMediumChanceOfCough();

        IStageAdditional WithHighChanceOfCough();

        IStageAdditional WithCustomChanceOfCough(int chancePercent);


        IStageAdditional WithLowChanceOfSneeze();

        IStageAdditional WithMediumChanceOfSneeze();

        IStageAdditional WithHighChanceOfSneeze();

        IStageAdditional WithCustomChanceOfSneeze(int chancePercent);


        IStageAdditional WithLowAdditionalStaminaDrain();

        IStageAdditional WithMediumAdditionalStaminaDrain();

        IStageAdditional WithHighAdditionalStaminaDrain();

        IStageAdditional WithCustomAdditionalStaminaDrain(float drainValue);


        IStageDisorder Disorders { get; }

        IStageDrainsNode NoDisorders();

    }
}
