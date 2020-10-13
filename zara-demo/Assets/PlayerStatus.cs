using ZaraEngine.Player;

public class PlayerStatus : IPlayerStatus {

    public bool IsUnderWater => false;
    public bool IsWalking => false;
    public bool IsStanding => true;
    public bool IsSwimming => false;
    public bool IsLimping => false;

    public float RunSpeed => 10f;
    public float WalkSpeed => 7f;
    public float CrouchSpeed => 5f;

}