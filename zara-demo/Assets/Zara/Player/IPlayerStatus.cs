using System;

namespace ZaraEngine.Player {

    public interface IPlayerStatus {

        bool IsUnderWater { get; }
        bool IsWalking { get; }
        bool IsStanding { get; }
        bool IsSwimming { get; }
        bool IsLimping { get; set; }

        float RunSpeed { get; }
        float WalkSpeed { get; }
        float CrouchSpeed { get; }

    }

}