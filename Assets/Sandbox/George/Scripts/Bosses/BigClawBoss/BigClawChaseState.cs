using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawChaseState : BossChaseState
{
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        base.OnStateEnter(_stateMachine);
        _stateMachine.agent.SetDestination(_stateMachine.playerTransform.position);
        _stateMachine.agent.isStopped = false;
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        Debug.Log("Exiting chase state");
       // _stateMachine.agent.ResetPath();
    }

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        if(Vector3.Distance(_stateMachine.transform.position,_stateMachine.playerTransform.position) >= _stateMachine.attackRange)
        {
            _stateMachine.agent.SetDestination(_stateMachine.playerTransform.position);
        } else
        {
            _stateMachine.agent.isStopped = true;
            _stateMachine.ChangeState(_stateMachine.attackState);
        }

    }
}
