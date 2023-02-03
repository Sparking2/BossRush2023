using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBossIdleState : BossIdleState
{
    // Idle shoot
    BoomerangBoss boomerangBoss;
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        if (!boomerangBoss) boomerangBoss = _stateMachine.gameObject.GetComponent<BoomerangBoss>();
        base.OnStateEnter(_stateMachine);
        //idleCounter = _stateMachine.bossBase.isBerserker ? _stateMachine.bossStats.bersekerWait : _stateMachine.bossStats.idleTime;
        //_stateMachine.bossState = BossStateMachine.BossState.idle;
        //waitCounter = _stateMachine.bossStats.GetWaitBeforeWander(_stateMachine.bossBase.isBerserker);

        wanderDestination = SetInitialWanderPoint(_stateMachine);

        _stateMachine.agent.isStopped = false;
        _stateMachine.agent.SetDestination(wanderDestination);
        _stateMachine.agent.speed = _stateMachine.bossStats.idleMovementSpeed;
        _stateMachine.agent.acceleration = _stateMachine.agent.speed;

    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        if(_stateMachine.agent.remainingDistance <= 1.0f)
        {
            //_stateMachine.agent.SetDestination(_stateMachine.transform.position);
            _stateMachine.agent.isStopped = true;
            if (waitCounter > 0.0f) waitCounter -= Time.deltaTime;
            else
            {
                _stateMachine.agent.isStopped = false;
                waitCounter = _stateMachine.bossStats.GetWaitBeforeWander(_stateMachine.bossBase.isBerserker);
                wanderDestination = _stateMachine.GetWanderPoint();
                _stateMachine.agent.SetDestination(wanderDestination);
            }
        }
        base.OnStateUpdate(_stateMachine);
       
        // ------------------------------ SHOOTING
        //if (shootTimer > 0.0f) shootTimer -= Time.deltaTime;
        //else
        //{
        //    shootTimer = GetRandomShootTime(_stateMachine);
        //    _stateMachine.bossBase.PerformSpecialAttack();
        //}
        //if (Vector3.Distance(_stateMachine.transform.position,wanderDestination) <= 0.1f)
        //{
        //    wanderDestination = _stateMachine.GetWanderPoint();
        //  //  _stateMachine.agent.SetDestination(wanderDestination);
        //    Debug.Log("Reached my destination");
        //} else
        //{
        //    Debug.Log("Moving to: " + wanderDestination);
        //    _stateMachine.agent.SetDestination(wanderDestination);
        //}
    }

    public override void OnIdleFinished(BossStateMachine _stateMachine)
    {
        if (!boomerangBoss.hasBoomerang)
        {
            _stateMachine.agent.isStopped = false;
            waitCounter = _stateMachine.bossStats.GetWaitBeforeWander(_stateMachine.bossBase.isBerserker);
            wanderDestination = _stateMachine.GetWanderPoint();
            _stateMachine.agent.SetDestination(wanderDestination);
            idleCounter = _stateMachine.bossBase.isBerserker ? _stateMachine.bossStats.bersekerWait : _stateMachine.bossStats.idleTime;
            return;
        }
        _stateMachine.ChangeState(_stateMachine.attackStates[0]);
    }
}
