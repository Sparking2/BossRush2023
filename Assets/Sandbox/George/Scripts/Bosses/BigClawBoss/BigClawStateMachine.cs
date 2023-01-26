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
        meleeAttackState = new BigClawMeleeAttackState();
        rangedAttackState = new BigClawRangedAttackState();
        tackleState = new BigClawTackleState();
        chargedAttackState = new BigClawChargedAttackState();

        //attackStates = new BossAttackState[4];
        //attackStates[0] = new BigClawMeleeAttackState();
    }
}
