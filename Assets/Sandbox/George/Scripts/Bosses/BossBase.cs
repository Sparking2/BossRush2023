using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ComponentHealth))]
public abstract class BossBase : MonoBehaviour
{
    public BossStatsScriptables statsScriptables;
    [HideInInspector] public ComponentHealth componentHealth;

    [HideInInspector] public Transform playerPosition;
    private void Awake()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        componentHealth = GetComponent<ComponentHealth>();

    }

    private void Start()
    {
        if (!statsScriptables) Debug.LogWarning("No stats scriptable found on the entity");
        if (componentHealth) componentHealth.SetHealth(statsScriptables.maxHealth);
    }
}
