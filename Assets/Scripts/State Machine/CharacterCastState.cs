using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCastState : CharacterBaseState
{
    public CharacterCastState(CharacterStateManager currentContext, CharacterStateFactory characterStateFactory) : base(currentContext, characterStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        HandleCast();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
      
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool(Ctx.IsCastingHash, false);
    }

    public override void CheckSwitchStates()
    {
       if (!Ctx.IsCastPressed)
        {
            SwitchState(Factory.Grounded());
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

    void HandleCast()
    {
        Ctx.Animator.SetBool(Ctx.IsCastingHash, true);
    }
}
