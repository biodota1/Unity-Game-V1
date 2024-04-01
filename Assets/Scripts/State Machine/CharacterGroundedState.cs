using UnityEngine;

public class CharacterGroundedState : CharacterBaseState
{
    public CharacterGroundedState(CharacterStateManager currentContext, CharacterStateFactory characterStateFactory) : base(currentContext, characterStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }


    public override void EnterState()
    {
        Ctx.CurrentMovementY = Ctx.GroundedGravity;
        Ctx.AppliedMovementY = Ctx.GroundedGravity;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsEscapePressed)
        {
            SwitchState(Factory.Escape());
        }
        else if (Ctx.IsCastPressed)
        {
            SwitchState(Factory.Cast());
        }

    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }
    }

  

   
}
