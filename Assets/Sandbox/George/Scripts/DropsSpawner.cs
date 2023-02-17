using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsSpawner : MonoBehaviour
{
    public static DropsSpawner Instance { get; private set; }
    [SerializeField] private float spawnTime;
    [SerializeField] private float spawnRadius;
    [SerializeField] private GameObject dropsPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnDrop()
    {
        Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPos.y = transform.position.y;
        Instantiate(dropsPrefab, spawnPos, Quaternion.identity);
        Invoke("SpawnDrop", spawnTime);
    }
}
