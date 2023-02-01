using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawTackleState : BossAttackState
{
    private float originalSpeed;
    private float originalAcceleration;

    private float tackleSpeed;
    private float tackleChannel;
    private float baseChannelTime = 2.0f;
    private int totalTackles = 3;
    BigClawBoss clawBoss;
    public override void OnStateEnter(BossStateMachine _stateMachine)
    {
        if (!clawBoss) clawBoss = _stateMachine.gameObject.GetComponent<BigClawBoss>();
        doOnFixed = true;
       // _stateMachine.animator.SetBool("tackleAtk", true);
        _stateMachine.animator.SetTrigger("tackleStart");

        originalSpeed = _stateMachine.agent.speed;
        originalAcceleration = _stateMachine.agent.acceleration;

        //_stateMachine.agent.isStopped = true;
        _stateMachine.agent.ResetPath();
        _stateMachine.agent.speed = 0.0f;
        _stateMachine.agent.acceleration = 35.0f;
       // _stateMachine.agent.updatePosition = false;
        tackleChannel = baseChannelTime;

        tackleSpeed = _stateMachine.bossStats.idleMovementSpeed + 15.0f;

        totalTackles = 3;
        _stateMachine.bossBase.LookAtPlayer();
        base.OnStateEnter(_stateMachine);
    }

    public override void OnStateExit(BossStateMachine _stateMachine)
    {
        _stateMachine.agent.speed = originalSpeed;
        _stateMachine.agent.acceleration = originalAcceleration;
    }

    private Vector3 target;

    public override void OnStateUpdate(BossStateMachine _stateMachine)
    {
        if (tackleChannel > 0f)
        {

            //_stateMachine.agent.speed = 0.0f;
            Debug.Log("Channeling");
            _stateMachine.agent.isStopped = true;
            _stateMachine.bossBase.LookAtPlayer();
            tackleChannel -= Time.deltaTime;
            target = _stateMachine.GetTargetPoint(_stateMachine.playerTransform.position);
            clawBoss.SetDangerLine();
        }
        else
        {
            Debug.Log("Tackling");
            _stateMachine.agent.isStopped = false;
            _stateMachine.agent.speed = tackleSpeed;  
            _stateMachine.agent.SetDestination(target);

            if (Vector3.Distance(_stateMachine.transform.position,target) <= 0.5f)
            {
                Debug.Log("Reach the target");
                if(totalTackles <= 0)
                {
                    Debug.Log("Finished the attack");
                    _stateMachine.ChangeState(_stateMachine.idleState);
                    _stateMachine.animator.SetTrigger("tackleEnd");
                } else
                {
                    Debug.Log("Doing it again...");
                    _stateMachine.agent.isStopped = true;
                    tackleChannel = baseChannelTime;
                    totalTackles--;
                }
            }

        }

    }
}
