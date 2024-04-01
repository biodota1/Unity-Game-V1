/*using System.Collections;
using UnityEngine;

public class CharacterJumpState : CharacerBaseState
{

    IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        Ctx.JumpCount = 0;
    }

    public CharacterJumpState(CharacterStateManager currentContext, CharacterStateFactory characterStateFactory) : base(currentContext, characterStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        HandleJump();

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleGravity();
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);
        if (Ctx.IsJumpPressed)
        {
            Ctx.RequireNewJumpPress = true;
        }

        Ctx.CurrentJumpResetRoutine = Ctx.StartCoroutine(IJumpResetRoutine());
        if (Ctx.JumpCount == 3)
        {
            Ctx.JumpCount = 0;
        }
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.Controller.isGrounded)
        {
            SwitchState(Factory.Grounded());
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

    void HandleJump()
    {
        if (Ctx.JumpCount < 3 && Ctx.CurrentJumpResetRoutine != null)
        {
            Ctx.StopCoroutine(Ctx.CurrentJumpResetRoutine);
        }
        Ctx.Animator.SetBool(Ctx.IsJumpingHash, true);
        Ctx.JumpCount += 1;
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
    }

    void HandleGravity()
    {
        bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;
        if (isFalling)
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f, -20.0f);
        }
        else
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.AppliedMovementY) * .5f;
        }
    }
}
*/