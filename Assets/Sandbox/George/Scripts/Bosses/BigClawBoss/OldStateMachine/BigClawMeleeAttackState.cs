using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawMeleeAttackState : BossAttackState
{
    private float originalSpeed;
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        originalSpeed = _stateMachine.agent.speed;
        _stateMachine.agent.speed = 0.0f;
        _stateMachine.animator.SetTrigger("meleeAtk");
        _stateMachine.agent.isStopped = true;

        base.OnStateEnter(_stateMachine);
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        _stateMachine.agent.isStopped = false;
        _stateMachine.agent.speed = originalSpeed;
        _stateMachine.animator.SetBool("isMoving", false);
    }

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {

    }
}
