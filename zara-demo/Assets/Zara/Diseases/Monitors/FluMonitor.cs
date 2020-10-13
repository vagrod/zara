using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Inventory;

namespace ZaraEngine.Diseases
{
    public class FluMonitor : DiseaseMonitorBase
    {

        private const float WamthLevelToCatchFluWithLowChance    = -6f;
        private const float WamthLevelToCatchFluWithMediumChance = -14f;
        private const float WamthLevelToCatchFluWithHighChance   = -23f;  

        private const int MonitorCheckInterval      = 25;     // Game minutes

        public const int FluDelayMinutesMin         = 35;     // Game minutes
        public const int FluDelayMinutesMax         = 2 * 60; // Game minutes

        private const int FluHighChance             = 50;     // Percents
        private const int FluMediumChance           = 25;     // Percents
        private const int FluLowChance              = 5;      // Percents
        private const int FluChanceNightBonus       = 5;      // Percents
        private const int WetnessLevelToConsiderFlu = 50;     // Percents
        private const int AnginaBonus               = 5;      // Percents

        // TODO: LOAD THESE FIELDS FROM SAVE

        private DateTime? _nextCheckTime;

        public FluMonitor(IGameController gc) : base(gc) { }

        public override void Check()
        {
            if (!_gc.WorldTime.HasValue)
                return;

            if (_nextCheckTime.HasValue)
            {
                if (_gc.WorldTime.Value >= _nextCheckTime.Value)
                {
                    _nextCheckTime = null;
                }
                else return;
            } else
            {
                _nextCheckTime = _gc.WorldTime.Value.AddMinutes(MonitorCheckInterval);

                return;
            }

            var activeFlu = _gc.Health.Status.GetActiveOrScheduled<Flu>(_gc.WorldTime.Value) as ActiveDisease;
            var hasAngina = _gc.Health.Status.HasActive<Angina>();

            if ((activeFlu != null && !activeFlu.IsHealing) || (activeFlu != null && activeFlu.IsSelfHealing))
                return;

            if (_gc.Body.IsWet && _gc.Body.WetnessLevel > WetnessLevelToConsiderFlu)
            {
                if (_gc.Body.WarmthLevelCached <= WamthLevelToCatchFluWithHighChance)
                {
                    var chance = FluHighChance;

                    if (_gc.TimeOfDay == TimesOfDay.Night)
                        chance += FluChanceNightBonus;

                    if (hasAngina)
                        chance += AnginaBonus;

                    if (chance.WillHappen())
                    {
                        if (activeFlu != null)
                        {
                            // Resume flu that currently is being healed
                            Events.NotifyAll(l => l.FluReTriggered());

                            activeFlu.InvertBack();
                            return;
                        }

                        //("You were wet in a cold weather. Flu will become active at " + _nextCheckTime.Value);

                        _nextCheckTime = _gc.WorldTime.Value.AddMinutes(Helpers.RollDice(FluDelayMinutesMin, FluDelayMinutesMax));
                        _gc.Health.Status.ActiveDiseases.Add(new ActiveDisease(_gc, typeof(Flu), _nextCheckTime.Value));
                    }
                }

                if (_gc.Body.WarmthLevelCached <= WamthLevelToCatchFluWithMediumChance)
                {
                    var chance = FluMediumChance;

                    if (_gc.TimeOfDay == TimesOfDay.Night)
                        chance += FluChanceNightBonus;

                    if (hasAngina)
                        chance += AnginaBonus;

                    if (chance.WillHappen())
                    {
                        //("You were wet in a cold weather. Flu is active now.");

                        if (activeFlu != null)
                        {
                            // Resume flu that currently is being healed
                            Events.NotifyAll(l => l.FluReTriggered());

                            activeFlu.InvertBack();
                            return;
                        }

                        _nextCheckTime = _gc.WorldTime.Value.AddMinutes(Helpers.RollDice(FluDelayMinutesMin, FluDelayMinutesMax));
                        _gc.Health.Status.ActiveDiseases.Add(new ActiveDisease(_gc, typeof(Flu), _nextCheckTime.Value));
                    }
                }

                if (_gc.Body.WarmthLevelCached <= WamthLevelToCatchFluWithLowChance)
                {
                    var chance = FluLowChance;

                    if (_gc.TimeOfDay == TimesOfDay.Night)
                        chance += FluChanceNightBonus;

                    if (hasAngina)
                        chance += AnginaBonus;

                    if (chance.WillHappen())
                    {
                        //("You were wet in a cold weather. Flu is active now.");

                        if (activeFlu != null)
                        {
                            // Resume flu that currently is being healed
                            Events.NotifyAll(l => l.FluReTriggered());

                            activeFlu.InvertBack();
                            return;
                        }

                        _nextCheckTime = _gc.WorldTime.Value.AddMinutes(Helpers.RollDice(FluDelayMinutesMin, FluDelayMinutesMax));
                        _gc.Health.Status.ActiveDiseases.Add(new ActiveDisease(_gc, typeof(Flu), _nextCheckTime.Value));
                    }
                }
            }
        }

        public override void OnConsumeItem(InventoryConsumableItemBase item)
        {
            
        }
    }
}
