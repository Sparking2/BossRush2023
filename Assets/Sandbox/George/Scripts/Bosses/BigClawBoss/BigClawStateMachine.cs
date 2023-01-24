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
    }
}
