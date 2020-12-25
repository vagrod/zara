using System;
using ZaraEngine.Diseases;

namespace ZaraEngine.StateManaging
{
    public class ChangedVitalsInfoSnippet : SnippetBase
    {

        public ChangedVitalsInfoSnippet() : base() { }
        public ChangedVitalsInfoSnippet(object contract) : base(contract) { }

        #region Data Fields

        public DiseaseLevels Level { get; set; }
        public float? InitialHeartRate { get; set; }
        public float? InitialBloodPressureTop { get; set; }
        public float? InitialBloodPressureBottom { get; set; }
        public float? InitialBodyTemperature { get; set; }
        public TimeSpan InitialStageDuration { get; set; }
        public bool IsEmpty => (InitialStageDuration == null || InitialStageDuration == default(TimeSpan)) && (!InitialHeartRate.HasValue || InitialHeartRate.Value == 0) && (!InitialBloodPressureTop.HasValue || InitialBloodPressureTop.Value == 0) && (!InitialBloodPressureBottom.HasValue || InitialBloodPressureBottom.Value == 0) && (!InitialBodyTemperature.HasValue || InitialBodyTemperature.Value == 0);

        #endregion

        public override object ToContract()
        {
            var c = new ChangedVitalsInfoContract
            {
                Level = (int)this.Level,
                InitialHeartRate = this.InitialHeartRate,
                InitialBloodPressureTop = this.InitialBloodPressureTop,
                InitialBloodPressureBottom = this.InitialBloodPressureBottom,
                InitialBodyTemperature = this.InitialBodyTemperature,
                InitialStageDuration = new TimeSpanContract(this.InitialStageDuration)
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ChangedVitalsInfoContract)o;

            Level = (DiseaseLevels)c.Level;
            InitialHeartRate = c.InitialHeartRate;
            InitialBloodPressureTop = c.InitialBloodPressureTop;
            InitialBloodPressureBottom = c.InitialBloodPressureBottom;
            InitialBodyTemperature = c.InitialBodyTemperature;
            InitialStageDuration = c.InitialStageDuration.IsEmpty ? default(TimeSpan) : c.InitialStageDuration.ToTimeSpan();

            ChildStates.Clear();
        }

    }
}
