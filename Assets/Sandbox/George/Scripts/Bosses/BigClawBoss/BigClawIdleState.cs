using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawIdleState : BossIdleState
{

    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        base.OnStateEnter(_stateMachine);
        _stateMachine.agent.SetDestination(_stateMachine.GetWanderPoint());
 
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        _stateMachine.agent.isStopped = true;
        _stateMachine.agent.ResetPath();
    }
    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        if(_stateMachine.agent.remainingDistance <= 0.1f)
        {
            if (waitCounter > 0.0f) waitCounter -= Time.deltaTime;
            else
            {
                waitCounter = _stateMachine.bossStats.minWaitBeforeWander;
                _stateMachine.agent.SetDestination(_stateMachine.GetWanderPoint());
            }
        }
        base.OnStateUpdate(_stateMachine);
    }

    public override void OnIdleFinished(BossStateMachine _stateMachine)
    {
        _stateMachine.ChangeState(_stateMachine.chaseState);
    }
}
