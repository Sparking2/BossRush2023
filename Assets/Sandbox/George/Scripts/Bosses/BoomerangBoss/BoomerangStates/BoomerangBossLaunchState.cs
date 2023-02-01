using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBossLaunchState : BossAttackState
{
    private float waitTime;

    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        waitTime = 1.0f;
        base.OnStateEnter(_stateMachine);
        _stateMachine.animator.SetTrigger("launch");
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
      //  throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        if (waitTime > 0.0f) waitTime -= Time.deltaTime;
        else
        {
            _stateMachine.ChangeState(_stateMachine.idleState);
        }
    }
}
