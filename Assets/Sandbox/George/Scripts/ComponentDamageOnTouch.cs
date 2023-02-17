using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDamageOnTouch : MonoBehaviour
{
    public float damage = 5;
    public bool destroyOnTouch;

    private bool canDealDamage = true;
    private float t = 0.1f;
    private void Update()
    {
        if (!canDealDamage)
        {
            if (t > 0f) t -= Time.deltaTime;
            else canDealDamage = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!canDealDamage) return;
        if (other.TryGetComponent(out ComponentHealth health))
        {
            health.ReduceHealth(damage);
        }
        canDealDamage = false;
        t = 1f;
        if (destroyOnTouch) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage) return;
        if (other.TryGetComponent(out ComponentHealth health))
        {
            health.ReduceHealth(damage);
        }
        canDealDamage = false;
        t = .1f;

        if (destroyOnTouch) Destroy(gameObject);
    }
}
