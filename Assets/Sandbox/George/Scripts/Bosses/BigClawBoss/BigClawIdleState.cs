using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawIdleState : BossIdleState
{

    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        base.OnStateEnter(_stateMachine);
        wanderDestination = Random.insideUnitSphere * _stateMachine.bossStats.GetWanderRadius();
        wanderDestination += _stateMachine.transform.position;
        _stateMachine.agent.SetDestination(wanderDestination);
        _stateMachine.agent.speed = _stateMachine.bossStats.movementSpeed;
        _stateMachine.animator.SetBool("isMoving", true);
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        _stateMachine.agent.isStopped = true;
        _stateMachine.agent.ResetPath();
        _stateMachine.animator.SetBool("isMoving", false);
    }
    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        if(_stateMachine.agent.remainingDistance <= 0.5f)
        {
            _stateMachine.animator.SetBool("isMoving", false);
            if (waitCounter > 0.0f) waitCounter -= Time.deltaTime;
            else
            {
                _stateMachine.animator.SetBool("isMoving", true);
                waitCounter = _stateMachine.bossStats.GetWaitBeforeWander();
                wanderDestination = _stateMachine.GetWanderPoint();
                _stateMachine.agent.SetDestination(wanderDestination);
            }
            //if (waitCounter > 0.0f) waitCounter -= Time.deltaTime;
            //else
            //{
            //    waitCounter = _stateMachine.bossStats.GetWaitBeforeWander();
            //    _stateMachine.agent.isStopped = false;
            //    _stateMachine.agent.SetDestination(_stateMachine.GetWanderPoint());
            //}
        }
        base.OnStateUpdate(_stateMachine);
    }

    public override void OnIdleFinished(BossStateMachine _stateMachine)
    {
        int attackIndex = Random.Range(0, 3);
        _stateMachine.ChangeState(_stateMachine.rangedAttackState);
        //switch (attackIndex)
        //{
        //    case 0: // Chase attack
        //        _stateMachine.ChangeState(_stateMachine.chaseState);
        //        break;
        //    case 1: // Range Attack
        //        _stateMachine.ChangeState(_stateMachine.rangedAttackState);
        //        break;
        //    case 2: // Tackle Attack
        //        break;
        //}

    }
}
