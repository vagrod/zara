using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine
{
    public interface IGameEvent
    {

        Guid Id { get; set; }
        
        bool IsHappened { get; set; }

        bool AutoReset { get; set; }

        IGameEvent ChainedEvent { get; set; }

        IGameEvent RootEvent { get; }

        IGameEvent ParentEvent { get; }

        void AddChainNode(IGameEvent parent, IGameEvent root);

        bool ChainEnded();

        bool IsActive { get; }

        bool Check(float deltaTime);

        void Reset();

    }
}
