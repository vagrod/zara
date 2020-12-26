using System;
using ZaraEngine.Injuries;

namespace ZaraEngine.StateManaging
{
    public class MedicalBodyApplianceSnippet : SnippetBase
    {

        public MedicalBodyApplianceSnippet() : base() { }
        public MedicalBodyApplianceSnippet(object contract) : base(contract) { }

        #region Data Fields

        public Player.BodyParts BodyPart { get; set; }
        public Guid ItemId { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new MedicalBodyApplianceContract
            {
                BodyPart = (int)this.BodyPart,
                ItemId = this.ItemId.ToString()
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (MedicalBodyApplianceContract)o;

            BodyPart = (Player.BodyParts)c.BodyPart;
            ItemId = Guid.Parse(c.ItemId);

            ChildStates.Clear();
        }

    }
}
