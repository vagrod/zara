using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Inventory;

namespace ZaraEngine.Diseases
{
    public class FoodPoisoningMonitor : DiseaseMonitorBase
    {

        public const int UnsafeWaterFoodPoisoningChance = 60; // percents

        public FoodPoisoningMonitor(IGameController gc) : base(gc) { }

        public override void OnConsumeItem(InventoryConsumableItemBase item)
        {
            var shouldTriggerDisease = false;

            var water = item as WaterVesselItemBase;
            if (water != null)
            {
                if (!water.IsSafe)
                {
                    shouldTriggerDisease = UnsafeWaterFoodPoisoningChance.WillHappen();
                }
            }
            else
            {
                var food = item as FoodItemBase;
                if (food != null)
                {
                    // Check if food is spoiled
                    if (food.IsSpoiled)
                        shouldTriggerDisease = food.SpoiledChanceOfFoodPoisoning.WillHappen();
                    else
                        shouldTriggerDisease = food.GeneralChanceOfFoodPoisoning.WillHappen();
                }
                else
                {
                    shouldTriggerDisease = item.SpoiledChanceOfFoodPoisoning.WillHappen();
                }
            }
            
            if (shouldTriggerDisease)
            {
                var activeFoodPoisoning = _gc.Health.Status.GetActiveOrScheduled<FoodPoisoning>(_gc.WorldTime.Value) as ActiveDisease;

                if (activeFoodPoisoning != null && activeFoodPoisoning.IsHealing && !activeFoodPoisoning.IsSelfHealing)
                {
                    activeFoodPoisoning.InvertBack();
                    return;
                }
                if (activeFoodPoisoning != null)
                    return;

                _gc.Health.Status.ActiveDiseases.Add(new ActiveDisease(_gc, typeof(FoodPoisoning), _gc.WorldTime.Value));
            }
        }
    }
}
