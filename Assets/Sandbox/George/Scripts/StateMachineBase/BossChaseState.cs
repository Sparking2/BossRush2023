using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BaseState
{
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        Debug.Log("Entering chase state");
        _stateMachine.bossState = BossStateMachine.BossState.chasing;
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        throw new System.NotImplementedException();
    }
}
