using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawChaseState : BossChaseState
{
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        base.OnStateEnter(_stateMachine);
        _stateMachine.animator.SetTrigger("chase");
        _stateMachine.agent.SetDestination(_stateMachine.playerTransform.position);
        _stateMachine.agent.speed = _stateMachine.bossStats.chaseSpeed;
        _stateMachine.agent.isStopped = false;
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        Debug.Log("Exiting chase state");
        //_stateMachine.agent.ResetPath();
    }

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        if(_stateMachine.agent.remainingDistance <= _stateMachine.attackRange)
        {
            //_stateMachine.agent.isStopped = true;
            _stateMachine.ChangeState(_stateMachine.meleeAttackState);
        }

    }
}
