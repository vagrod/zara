using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class ClothesHealthEffectsSnippet : SnippetBase
    {

        public ClothesHealthEffectsSnippet() : base() { }
        public ClothesHealthEffectsSnippet(object contract) : base(contract) { }

        #region Data Fields

        public DateTime? LastClothesChangeTime { get; set; }
        public DateTime? LastAutoReLerpTime { get; set; }
        public float TargetBodyTemperatureDelta { get; set; }
        public float TargetHeartRateDelta { get; set; }
        public float CurrentTemperatureBonus { get; set; }
        public float CurrentHeartRateBonus { get; set; }
        public float PlayerRunSpeedBonus { get; set; }
        public float HeartRateBonus { get; set; }
        public float BodyTemperatureBonus { get; set; }
        public float StaminaBonus { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ClothesHealthEffectsContract
            {
                LastClothesChangeTime = this.LastClothesChangeTime,
                LastAutoReLerpTime = this.LastAutoReLerpTime,
                TargetBodyTemperatureDelta = this.TargetBodyTemperatureDelta,
                TargetHeartRateDelta = this.TargetHeartRateDelta,
                CurrentTemperatureBonus = this.CurrentTemperatureBonus,
                CurrentHeartRateBonus = this.CurrentHeartRateBonus,
                PlayerRunSpeedBonus = this.PlayerRunSpeedBonus,
                HeartRateBonus = this.HeartRateBonus,
                BodyTemperatureBonus = this.BodyTemperatureBonus,
                StaminaBonus = this.StaminaBonus
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ClothesHealthEffectsContract)o;

            LastClothesChangeTime = c.LastClothesChangeTime;
            LastAutoReLerpTime = c.LastAutoReLerpTime;
            TargetBodyTemperatureDelta = c.TargetBodyTemperatureDelta;
            TargetHeartRateDelta = c.TargetHeartRateDelta;
            CurrentTemperatureBonus = c.CurrentTemperatureBonus;
            CurrentHeartRateBonus = c.CurrentHeartRateBonus;
            PlayerRunSpeedBonus = c.PlayerRunSpeedBonus;
            HeartRateBonus = c.HeartRateBonus;
            BodyTemperatureBonus = c.BodyTemperatureBonus;
            StaminaBonus = c.StaminaBonus;

            ChildStates.Clear();
        }

    }
}
