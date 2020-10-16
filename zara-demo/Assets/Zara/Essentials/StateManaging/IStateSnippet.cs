using System;
using System.Collections.Generic;
using ZaraEngine;

namespace ZaraEngine.StateManaging {

    public interface IStateSnippet {

        Dictionary<string, IStateSnippet> ChildStates { get; }

        object ToContract();

        void FromContract(object o);

    }

}