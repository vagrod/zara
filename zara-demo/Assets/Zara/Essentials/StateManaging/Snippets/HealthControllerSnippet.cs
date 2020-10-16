using System.Collections.Generic;
using ZaraEngine;

namespace ZaraEngine.StateManaging {

    public class HealthControllerSnippet : IStateSnippet
    {

        public List<IStateSnippet> ChildStates { get; }

        public object ToContract()
        {
            return new HealthControllerStateContract();
        }

        public void FromContract(object o)
        {

        }

    }

}