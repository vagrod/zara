using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine.Diseases.Stages.Fluent
{
    public interface IStageDisorder
    {

        IStageDisorder WithSleepDisorder();

        IStageDisorder WithFoodDisgust();

        IStageDisorder WillNotBeAbleToRun();

        IStageDrainsNode NotDeadly();

        IStageDrainsNode WithLowRiskOfDeath();

        IStageDrainsNode WithMediumRiskOfDeath();

        IStageDrainsNode WithHighRiskOfDeath();

        IStageDrainsNode WithCustomRiskOfDeath(int percentChance);

    }
}
