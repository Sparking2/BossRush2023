using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawChaseState : BossChaseState
{
    private float baseAcceleration;
    private float baseAngularSpeed;

    private float chaseWaitTime;
    private Vector3 target;
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        base.OnStateEnter(_stateMachine);
        baseAngularSpeed = _stateMachine.agent.angularSpeed;
        baseAcceleration = _stateMachine.agent.acceleration;

        _stateMachine.animator.SetTrigger("chase");
        target = _stateMachine.playerTransform.position;

        _stateMachine.bossBase.LookAtPlayer();

        _stateMachine.agent.isStopped = true;
        _stateMachine.agent.SetDestination(target);
        _stateMachine.agent.speed = _stateMachine.bossBase.isBerserker ? _stateMachine.bossStats.berseckerSpeed : _stateMachine.bossStats.chaseSpeed;

        _stateMachine.agent.angularSpeed = 500;
        _stateMachine.agent.acceleration = _stateMachine.agent.speed;

        chaseWaitTime = 0.5f;
    }



    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        _stateMachine.agent.acceleration = baseAcceleration;
        _stateMachine.agent.angularSpeed = baseAngularSpeed;
    }

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        chaseWaitTime -= Time.deltaTime;
        if (chaseWaitTime >= 0.0f) return;
        chaseWaitTime = 0f;
        _stateMachine.agent.isStopped = false;

        if (Vector3.Distance(_stateMachine.transform.position, target) >= 2f) return;
        //_stateMachine.agent.isStopped = true;
        _stateMachine.agent.acceleration = 750f;
        //_stateMachine.agent.ResetPath();
        //_stateMachine.agent.SetDestination(_stateMachine.transform.position);
        _stateMachine.ChangeState(_stateMachine.attackStates[0]);
    }
}
