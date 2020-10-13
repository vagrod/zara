using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Inventory;

namespace ZaraEngine.Diseases
{
    public class AnginaMonitor : DiseaseMonitorBase
    {

        private const int MinutesFromBoilMediumTemperatureToConsiderAngina   = 60 * 5; // Game minutes
        private const int MinutesFromBoilColdTemperatureToConsiderAngina     = 60 * 1; // Game minutes
        private const int MinutesFromBoilVeryColdTemperatureToConsiderAngina = 25;     // Game minutes

        public const int NoAnginaTemperature = 18; // C and higher
        public const int MediumTemperature   = 11; // C and higher
        public const int ColdTemperature     = 5;  // C and higher
        public const int VeryColdTemperature = -1; // C and higher

        public const int AnginaDelayMinutesMin = 25;     // Game minutes
        public const int AnginaDelayMinutesMax = 1 * 60; // Game minutes
        private const int AnginaHighChance     = 54;     // Percents
        private const int AnginaMediumChance   = 25;     // Percents
        private const int AnginaLowChance      = 5;      // Percents
        private const int FluBonus             = 5;      // Percents

        public AnginaMonitor(IGameController gc) : base(gc) { }

        public override void Check()
        {
            
        }

        public override void OnConsumeItem(InventoryConsumableItemBase item)
        {
            if (_gc.Weather.Temperature >= NoAnginaTemperature)
                return;

            var water = item as WaterVesselItemBase;

            if (water == null)
                return;

            var activeAngina = _gc.Health.Status.GetActiveOrScheduled<Angina>(_gc.WorldTime.Value) as ActiveDisease;
            var hasFlu = _gc.Health.Status.HasActive<Flu>();

            if ((activeAngina != null && !activeAngina.IsHealing) || (activeAngina != null && activeAngina.IsSelfHealing))
                return;

            var mustTriggerDisease = false;
            var minutesSinceBoled = water.LastBoilTime.HasValue ? (_gc.WorldTime.Value - water.LastBoilTime.Value).TotalMinutes : double.MaxValue;
            var fluBonus = hasFlu ? FluBonus : 0;

            if (_gc.Weather.Temperature <= VeryColdTemperature)
            {
                if (minutesSinceBoled >= MinutesFromBoilVeryColdTemperatureToConsiderAngina)
                {
                    if ((AnginaHighChance + fluBonus).WillHappen())
                    {
                        mustTriggerDisease = true;
                    }
                }
            }

            if (_gc.Weather.Temperature <= ColdTemperature)
            {
                if (minutesSinceBoled >= MinutesFromBoilColdTemperatureToConsiderAngina)
                {
                    if ((AnginaMediumChance + fluBonus).WillHappen())
                    {
                        mustTriggerDisease = true;
                    }
                }
            }

            if (_gc.Weather.Temperature <= MediumTemperature)
            {
                if (minutesSinceBoled >= MinutesFromBoilMediumTemperatureToConsiderAngina)
                {
                    if ((AnginaLowChance + fluBonus).WillHappen())
                    {
                        mustTriggerDisease = true;
                    }
                }
            }

            if (mustTriggerDisease)
            {
                var diseaseStime = _gc.WorldTime.Value.AddMinutes(Helpers.RollDice(AnginaDelayMinutesMin, AnginaDelayMinutesMax));
                _gc.Health.Status.ActiveDiseases.Add(new ActiveDisease(_gc, typeof(Angina), diseaseStime));
            }
        }
    }
}