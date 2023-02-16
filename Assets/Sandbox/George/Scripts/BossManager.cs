using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance { get; private set; }
    public GameObject[] bosses;
    [SerializeField] private int currentBoss = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void OnGameStart()
    {
        bosses[currentBoss].SetActive(true);
    }
    public void OnBossKilled()
    {
        // Spawn healings
        // Delayed next spawn
        currentBoss++;
        if(currentBoss < bosses.Length - 1)
        {
            Invoke("SpawnNewBoss", 3.5f);
        } else
        {
            // Do win UI
        }
    }

    private void SpawnNewBoss()
    {
        bosses[currentBoss].SetActive(true);
    }
}
