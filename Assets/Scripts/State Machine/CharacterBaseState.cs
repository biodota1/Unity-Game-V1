

public abstract class CharacterBaseState
{
    private bool _isRootState = false;
    private CharacterStateManager _ctx;
    private CharacterStateFactory _factory;
    private CharacterBaseState _currentSubState;
    private CharacterBaseState _currentSuperState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected CharacterStateManager Ctx { get { return _ctx; } }
    protected CharacterStateFactory Factory { get { return _factory; } }
    public CharacterBaseState(CharacterStateManager currentContext, CharacterStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();
    
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if(_currentSubState != null )
        {
            _currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(CharacterBaseState newState)
    {
        ExitState();

        newState.EnterState();

        if(_isRootState )
        {
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }


    }

    protected void SetSuperState(CharacterBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(CharacterBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
