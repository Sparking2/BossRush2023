using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawStateMachine : BossStateMachine
{
    public void OnAttackFinished()
    {
        ChangeState(idleState);
    }

    public override void InitializeStates()
    {
        idleState = new BigClawIdleState();
        chaseState = new BigClawChaseState();

        attackStates = new BossAttackState[4];
        attackStates[0] = new BigClawMeleeAttackState();
        attackStates[1] = new BigClawRangedAttackState();
        attackStates[2] = new BigClawTackleState();
        attackStates[3] = new BigClawChargedAttackState();
    }
}
