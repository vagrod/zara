using ZaraEngine.Player;

public partial class PlayerStatus : IPlayerStatus
{
    private bool _isStanding = true;
    private bool _isWalking;
    private bool _isSwimming;
    private bool _isRunning;
    private bool _isLimping;
    private bool _isUnderWater;
    private float _runSpeed;
    private float _walkSpeed;
    private float _crouchSpeed;

    public bool IsUnderWater => _isUnderWater;
    public bool IsWalking => _isWalking;
    public bool IsStanding => _isStanding;
    public bool IsRunning => _isRunning;
    public bool IsSwimming => _isSwimming;
    public bool IsLimping => _isLimping;

    public float RunSpeed => _runSpeed;
    public float WalkSpeed => _walkSpeed;
    public float CrouchSpeed => _crouchSpeed;

    public void SetRunning(bool value)
    {
        if (value)
        {
            _isStanding = false;
            _isWalking = false;
            _isRunning = true;
        }
        else
        {
            _isStanding = true;
            _isWalking = false;
            _isRunning = false;
        }
    }

    public void SetWalking(bool value)
    {
        _isStanding = !value;
        _isWalking = value;
        _isRunning = false;
    }

    public void SetStanding(bool value)
    {
        _isWalking = !value;
        _isStanding = value;
        _isRunning = false;
    }

    public void SetWalkSpeed(float value)
    {
        _walkSpeed = value;
    }

    public void SetRunSpeed(float value)
    {
        _runSpeed = value;
    }

    public void SetCrouchSpeed(float value)
    {
        _crouchSpeed = value;
    }

    public void SetLimping(bool value)
    {
        _isLimping = value;
        _isStanding = false;
        _isRunning = false;
        _isWalking = false;
    }

    public void SetSwimming(bool value)
    {
        _isSwimming = value;
        _isStanding = false;
        _isRunning = false;
        _isWalking = false;
    }

    public void SetUnderwater(bool value)
    {
        _isUnderWater = value;
        _isStanding = false;
        _isRunning = false;
        _isWalking = false;
    }
}
