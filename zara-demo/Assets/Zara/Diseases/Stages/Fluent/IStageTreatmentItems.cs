namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageTreatmentItems
    {

        IStageTreatmentItemAction AndWithSpecialItems(params string[] items);

        IStageFinish AndWithoutSpecialItems();

    }
}
