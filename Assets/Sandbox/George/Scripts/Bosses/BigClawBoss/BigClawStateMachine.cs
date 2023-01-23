using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawStateMachine : BossStateMachine
{
    public override void InitializeStates()
    {
        idleState = new BigClawIdleState();
        chaseState = new BigClawChaseState();
        attackState = new BigClawAttackState();
    }
}
