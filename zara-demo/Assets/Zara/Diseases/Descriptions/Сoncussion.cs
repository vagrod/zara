using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Diseases.Stages.Fluent;

namespace ZaraEngine.Diseases
{
    public class Сoncussion : DiseaseDefinitionBase
    {

        public Сoncussion()
        {
            Name = "Сoncussion";
            Stages = new List<DiseaseStage>();
        }
    }
}
