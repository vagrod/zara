using System;
using System.Collections.Generic;
using ZaraEngine;

namespace ZaraEngine.StateManaging {

    public interface IStateSnippet {

        List<IStateSnippet> ChildStates { get; }

        object ToContract();

    }

}