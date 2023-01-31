using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBossStateMachine : BossStateMachine
{
    public override void InitializeStates()
    {
        idleState = new BoomerangBossIdleState();
        //chaseState = new BigClawChaseState();

        attackStates = new BossAttackState[1];
        attackStates[0] = new BoomerangBossLaunchState();
        //attackStates[0] = new BigClawMeleeAttackState();
        //attackStates[1] = new BigClawRangedAttackState();
        //attackStates[2] = new BigClawTackleState();
        //attackStates[3] = new BigClawChargedAttackState();
    }
}

