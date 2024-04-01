
using UnityEngine;

public class CharacterRunState : CharacterBaseState
{
    public CharacterRunState(CharacterStateManager currentContext, CharacterStateFactory characterStateFactory) : base (currentContext, characterStateFactory) { }

    public override void EnterState()
    {
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.RunMultiplier;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.RunMultiplier;
    }

    public override void ExitState() { }

    public override void CheckSwitchStates()
    {
        if(!Ctx.IsMovementPressed) {
            SwitchState(Factory.Idle());
        }
        else if(Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SwitchState(Factory.Walk());
        }
    }

    public override void InitializeSubState() { }
}
