using System;
using System.Collections.Generic;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Injuries;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{

    public enum DiseaseLevels
    {
        InitialStage = 2,
        Progressing = 3,
        Worrying = 4,
        Critical = 5,
        HealthyStage = 1
    }

    public abstract class DiseaseDefinitionBase: IAcceptsStateChange
    {

        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; protected set; }

        public List<DiseaseStage> Stages { get; protected set; }

        public bool RequiresBodyPart{ get; protected set; }

        public virtual void Check(ActiveDisease disease, IGameController gc)
        {
            // Optional for children
        }

        public virtual void OnResumeDisease()
        {
            // Optional for children
        }

        public void SwapChain(List<DiseaseStage> stages)
        {
            Stages = stages;
        }

        public virtual void InitializeWithInjury(Player.BodyParts initialInjury)
        {
            // Optional for children
        }

        #region State Manage

        public abstract IStateSnippet GetState();

        public abstract void RestoreState(IStateSnippet state);

        #endregion

    }

}
