using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class ActiveDiseasesAndInjuriesContract
    {

        public ActiveDiseaseContract[] ActiveDiseases;
        public ActiveInjuryContract[] ActiveInjuries;

    }
}
