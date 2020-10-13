using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageDrainsNode
    {

        IStageDrains Drain { get; }

        IStageTreatmentNode NoDrains();

    }

    public interface IStageDrains : IStageTreatmentNode
    {

        IStageDrains WaterPerSecond(float value);

        IStageDrains FoodPerSecond(float value);

        IStageDrains FatigueIncreasePerSecond(float value);


    }
}
