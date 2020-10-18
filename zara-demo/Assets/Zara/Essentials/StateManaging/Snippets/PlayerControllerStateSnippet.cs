using System;
using System.Collections.Generic;
using System.Linq;

namespace ZaraEngine.StateManaging
{
    public class PlayerControllerStateSnippet : SnippetBase
    {

        public PlayerControllerStateSnippet() : base() { }
        public PlayerControllerStateSnippet(object contract) : base(contract) { }

        public InventoryControllerStateSnippet InventoryData { get; private set; }

        public void SetInventoryData(InventoryControllerStateSnippet data)
        {
            InventoryData = data;
        }

        #region Data Fields

        public List<Guid> Clothes{ get; set; } = new List<Guid>() { };
        public List<MedicalBodyApplianceSnippet> Appliances{ get; set; } = new List<MedicalBodyApplianceSnippet>() { };

        public float WarmthLevelTimeoutCounter { get; set; }
        public float WetnessLevelTimeoutCounter { get; set; }
        public float WarmthLerpTarget { get; set; }
        public float? WarmthLerpCounter { get; set; }
        public float WarmthLerpBase { get; set; }
        public float SleepingCounter { get; set; }
        public float SleepDurationGameHours { get; set; }
        public float SleepHealthCheckPeriod { get; set; }
        public int SleepHealthChecksLeft { get; set; }
        public DateTime SleepStartTime { get; set; }
        public float FatigueValueAfterSleep { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new PlayerControllerStateContract
            {
                Clothes = this.Clothes.ToList().ConvertAll(x => x.ToString()).ToArray(),
                Appliances = this.Appliances.ToList().ConvertAll(x => (MedicalBodyApplianceContract)x.ToContract()).ToArray(),

                WarmthLevelTimeoutCounter = this.WarmthLevelTimeoutCounter,
                WetnessLevelTimeoutCounter = this.WetnessLevelTimeoutCounter,
                WarmthLerpTarget = this.WarmthLerpTarget,
                WarmthLerpCounter = this.WarmthLerpCounter,
                WarmthLerpBase = this.WarmthLerpBase,
                SleepingCounter = this.SleepingCounter,
                SleepDurationGameHours = this.SleepDurationGameHours,
                SleepHealthCheckPeriod = this.SleepHealthCheckPeriod,
                SleepHealthChecksLeft = this.SleepHealthChecksLeft,
                SleepStartTime = new DateTimeContract(this.SleepStartTime),
                FatigueValueAfterSleep = this.FatigueValueAfterSleep
            };

            c.WetnessController = (WetnessControllerContract)ChildStates["WetnessController"].ToContract();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (PlayerControllerStateContract)o;

            Clothes = c.Clothes.ToList().ConvertAll(Guid.Parse);
            Appliances = c.Appliances.ToList().ConvertAll(x => new MedicalBodyApplianceSnippet(x));

            WarmthLevelTimeoutCounter = c.WarmthLevelTimeoutCounter;
            WetnessLevelTimeoutCounter = c.WetnessLevelTimeoutCounter;
            WarmthLerpTarget = c.WarmthLerpTarget;
            WarmthLerpCounter = c.WarmthLerpCounter;
            WarmthLerpBase = c.WarmthLerpBase;
            SleepingCounter = c.SleepingCounter;
            SleepDurationGameHours = c.SleepDurationGameHours;
            SleepHealthCheckPeriod = c.SleepHealthCheckPeriod;
            SleepHealthChecksLeft = c.SleepHealthChecksLeft;
            SleepStartTime = c.SleepStartTime.ToDateTime();
            FatigueValueAfterSleep = c.FatigueValueAfterSleep;

            ChildStates.Clear();

            ChildStates.Add("WetnessController", new WetnessControllerSnippet(c.WetnessController));
        }

    }
}
