using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterEscapeState : CharacterBaseState
{
    public float dashSpeed = 10f; // Distance to dash
    public float dashDuration = 0.25f; // Duration of the dash
    public CharacterEscapeState(CharacterStateManager currentContext, CharacterStateFactory characterStateFactory) : base(currentContext, characterStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public IEnumerator IDash()
    {
       float startTime = Time.time;

        while(Time.time < startTime + dashDuration)
        {
            Vector3 positionToLookAt = new Vector3(Ctx.CurrentMovementInput.x, 0f, Ctx.CurrentMovementInput.y);
           
            Ctx.Controller.Move(positionToLookAt * dashSpeed * Time.deltaTime);
           
            yield return null;
        }
        Ctx.Animator.SetBool(Ctx.IsEscapeHash, false);
    }

    public override void EnterState()
    {
        HandleEscape();
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
        if(!Ctx.IsEscapePressed)
        {
            SwitchState(Factory.Grounded());
        }
        if (Ctx.IsCastPressed)
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

   

    void HandleEscape()
    {
        Ctx.DashCoroutine = Ctx.StartCoroutine(IDash());
        Ctx.Animator.SetBool(Ctx.IsEscapeHash, true);
    }
}
