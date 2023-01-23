using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class BossBase : MonoBehaviour
{
    public BossStatsScriptables statsScriptables;
     public ComponentHealth componentHealth;


    private void Awake()
    {
        componentHealth = new ComponentHealth();
    }

    private void Start()
    {
        if(componentHealth) componentHealth.SetHealth(statsScriptables.maxHealth);
    }
}
