using System.Collections.Generic;
using ZaraEngine;

namespace ZaraEngine.StateManaging {

    public class HealthControllerSnippet : IStateSnippet
    {

        public Dictionary<string, IStateSnippet> ChildStates { get; }

        public object ToContract()
        {
            return new HealthControllerStateContract();
        }

        public void FromContract(object o)
        {

        }

    }

}