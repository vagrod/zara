using ZaraEngine.Player;

public class PlayerStatus : IPlayerStatus {

    private bool _isStanding = true;
    private bool _isWalking;

    public bool IsUnderWater => false;
    public bool IsWalking => _isWalking;
    public bool IsStanding => _isStanding;
    public bool IsSwimming => false;
    public bool IsLimping => false;

    public float RunSpeed => 10f;
    public float WalkSpeed => 7f;
    public float CrouchSpeed => 5f;

    public void SetRunning(bool value){
        if(value){
            _isStanding = false;
            _isWalking = false;
        } else {
            _isStanding = true;
            _isWalking = false;
        }
    }

    public void SetWalking(bool value){
        if(value){
            _isStanding = false;
            _isWalking = true;
        } else {
            _isStanding = true;
            _isWalking = false;
        }
    }

}