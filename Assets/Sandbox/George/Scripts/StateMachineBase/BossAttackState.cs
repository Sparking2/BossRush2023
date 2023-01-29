using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BaseState
{
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        _stateMachine.bossState = BossStateMachine.BossState.attacking;
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
