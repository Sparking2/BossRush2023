using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDamageOnRay : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private bool canHit = true;
    RaycastHit hit;
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            if (!canHit) return;
            if(hit.transform.gameObject.TryGetComponent(out ComponentHealth playerHealth))
            {
                playerHealth.ReduceHealth(5);
            }
            canHit = false;
            Invoke("RestoreHit", 1.0f);
        }
    }

    private void RestoreHit()
    {
        canHit = true;
    }
}
