using System.Collections.Generic;

namespace ZaraEngine.StateManaging {

    public interface IStateSnippet {

        Dictionary<string, IStateSnippet> ChildStates { get; }

        object ToContract();

        void FromContract(object o);

    }

}