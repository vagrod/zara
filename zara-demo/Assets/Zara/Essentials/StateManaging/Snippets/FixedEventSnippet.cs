using System;
using System.Collections.Generic;

namespace ZaraEngine.StateManaging
{

    public class FixedEventSnippet : SnippetBase
    {

        public FixedEventSnippet() : base() { }
        public FixedEventSnippet(object contract) : base(contract) { }

        #region Data Fields

        public bool IsHappened { get; set; }
        public bool AutoReset { get; set; }

        #endregion 

        public override object ToContract()
        {
            return new FixedEventContract
            {
                IsHappened = this.IsHappened,
                AutoReset = this.AutoReset
            };
        }

        public override void FromContract(object o)
        {
            var c = (FixedEventContract)o;

            IsHappened = c.IsHappened;
            AutoReset = c.AutoReset;

            ChildStates.Clear();
        }

    }

}