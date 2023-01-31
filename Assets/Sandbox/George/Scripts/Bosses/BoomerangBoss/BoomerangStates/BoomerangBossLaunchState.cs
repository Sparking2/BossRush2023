using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBossLaunchState : BossAttackState
{
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        base.OnStateEnter(_stateMachine);
        _stateMachine.animator.SetTrigger("launch");
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
      //  throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        //throw new System.NotImplementedException();
    }
}
