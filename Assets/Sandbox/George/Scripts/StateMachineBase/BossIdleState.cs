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
        idleCounter = _stateMachine.bossBase.isBerserker? _stateMachine.bossStats.bersekerWait: _stateMachine.bossStats.idleTime;
        _stateMachine.bossState = BossStateMachine.BossState.idle;
        waitCounter = _stateMachine.bossStats.GetWaitBeforeWander(_stateMachine.bossBase.isBerserker);
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

    public Vector3 SetInitialWanderPoint(BossStateMachine _stateMachine)
    {
        Vector3 wanderPoint;
        wanderPoint = Random.insideUnitSphere * _stateMachine.bossStats.GetWanderRadius();
        wanderPoint += _stateMachine.transform.position;
        return wanderPoint;
    }
}
