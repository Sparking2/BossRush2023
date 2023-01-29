using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawRangedAttackState : BossAttackState
{
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        _stateMachine.agent.SetDestination(_stateMachine.transform.position);
        _stateMachine.animator.SetTrigger("rangedAtk");
        _stateMachine.agent.isStopped = true;
        _stateMachine.bossBase.LookAtPlayer();

        base.OnStateEnter(_stateMachine);
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        _stateMachine.agent.isStopped = false;
        _stateMachine.animator.SetBool("isMoving", false);
    }

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {

    }
}
