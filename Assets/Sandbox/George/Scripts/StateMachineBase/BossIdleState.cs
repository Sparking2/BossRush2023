using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BaseState
{
    public float waitCounter;
    public float idleCounter;
    public Vector3 wanderDestination;
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        idleCounter = _stateMachine.bossStats.idleTime;
        _stateMachine.bossState = BossStateMachine.BossState.idle;
        waitCounter = _stateMachine.bossStats.GetWaitBeforeWander();
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        if (idleCounter > 0.0f) idleCounter -= Time.deltaTime;
        else
        {
            OnIdleFinished(_stateMachine);
        }
    }

    public virtual void OnIdleFinished(BossStateMachine _stateMachine)
    {

    }
}
