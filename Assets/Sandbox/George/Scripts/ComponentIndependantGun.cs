using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentIndependantGun : MonoBehaviour
{
    [SerializeField] private bool gunActive;
    [SerializeField] private bool isBerseker;
    private float shootDelay;
    private float shootCounter;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject specialProjectilePrefab;
    public void SetShootSpeed(float _speed)
    {
        shootDelay = _speed;
        shootCounter = shootDelay;
    }

    public void SetGunState(bool _active)
    {
        gunActive = _active;
    }

    public void SetBersekerState(bool _active)
    {
        isBerseker = _active;
    }

    private void Update()
    {
        if (!gunActive) return;
        if (shootCounter > 0.0f) shootCounter -= Time.deltaTime;
        else
        {
            shootCounter = shootDelay;
            if (isBerseker)
            {
                Instantiate(specialProjectilePrefab, transform.position, transform.rotation);
            } else  Instantiate(projectilePrefab, transform.position, transform.rotation);
        }
    }
}


