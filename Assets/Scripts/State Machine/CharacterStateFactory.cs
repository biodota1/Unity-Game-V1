

using TMPro.EditorUtilities;

public class CharacterStateFactory
{
    CharacterStateManager _context;

    public CharacterStateFactory(CharacterStateManager currentContext)
    {
        _context = currentContext;
    }
    public CharacterBaseState Cast()
    {
        return new CharacterCastState(_context, this);
    }
    public CharacterBaseState Idle()
    {
        return new CharacterIdleState(_context, this);
    }
    public CharacterBaseState Walk()
    {
        return new CharacterWalkState(_context, this);
    }
    public CharacterBaseState Run()
    {
        return new CharacterRunState(_context, this);
    }
    public CharacterBaseState Escape()
    {
        return new CharacterEscapeState(_context, this);
    }
    public CharacterBaseState Grounded()
    {
        return new CharacterGroundedState(_context, this);
    }
    

}
