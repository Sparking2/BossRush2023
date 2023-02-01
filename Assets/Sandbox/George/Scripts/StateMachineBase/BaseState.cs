using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public bool doOnFixed = false;
    public abstract void OnStateEnter(BossStateMachine _stateMachine);
    public abstract void OnStateUpdate(BossStateMachine _stateMachine);

    public abstract void OnStateExit(BossStateMachine _stateMachine);
}
