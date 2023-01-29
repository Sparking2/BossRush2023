using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHealth : MonoBehaviour
{
    [Header("Health component: "), Space(10)]
    [SerializeField] private float health;

    public void SetHealth(float _health)
    {
        health = _health;
    }
}
