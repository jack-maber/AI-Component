using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;

    public void UpdatePushing(bool newIsPushing)
    {
        animator.SetBool("Pushing", newIsPushing);
    }

    public void UpdateBraking(bool newIsBraking)
    {
        animator.SetBool("Braking", newIsBraking);
    }

    public void UpdateFalling(bool newIsFalling)
    {
        //animator.SetBool("Falling", newIsFalling);
    }

    public void TriggerTaunt(int tauntID = 0)
    {
        animator.SetInteger("TauntID", tauntID);
        animator.SetTrigger("Taunt");
    }

    public void DistFromGroundUpdate(float dist)
    {
        animator.SetFloat("DistFromGround", dist);
    }

    public void TriggerItemLayer()
    {
        animator.SetTrigger("Item");
    }

    public void TriggerThrowForward()
    {
        animator.SetTrigger("Throw_For");
    }

    public void UpdateBoost(bool newIsBoosting)
    {
        animator.SetBool("Boosting", newIsBoosting);
    }

    public void TriggerWin()
    {
        animator.SetTrigger("Win");
    }

    public void TriggerLose()
    {
        animator.SetTrigger("Lose");
    }

    public void TriggerPerfPush()
    {
        animator.SetTrigger("Perf_Push");
    }

    public void TriggerPoorPush()
    {
        animator.SetTrigger("Poor_Push");       
    }

    public void TriggerDamage()
    {
        animator.SetTrigger("Damage");
    }
}
