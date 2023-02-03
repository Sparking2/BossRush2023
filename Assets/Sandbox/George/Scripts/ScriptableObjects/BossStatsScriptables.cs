using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Stats",menuName = "Boss stats")]
public class BossStatsScriptables : ScriptableObject
{
    [Header("General Stats"),Space(10)]
    public int maxHealth;
    public float idleMovementSpeed;
    [Header("Idle state parameters: "),Space(10)]
    public float idleTime;
    public float minWanderRadius;
    public float maxWanderRadius;

    public float minWaitBeforeWander;
    public float maxWaitBeforeWander;

    public float bersekerWait = .5f;

    [Header("Chase state parameters: "), Space(10)]
    public float chaseTime;
    public float chaseSpeed;
    public float berseckerSpeed;
    public float attackTriggerRadius;

    public float GetWanderRadius()
    {
        return Random.Range(minWanderRadius, maxWanderRadius);
    }

    public float GetWaitBeforeWander(bool _berseker)
    {

        return _berseker?bersekerWait: Random.Range(minWaitBeforeWander, maxWaitBeforeWander);
    }
}
