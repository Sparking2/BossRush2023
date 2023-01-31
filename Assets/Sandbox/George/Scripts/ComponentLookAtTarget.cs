using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentLookAtTarget : MonoBehaviour
{
    public bool canLookAtTarget = false;
    [SerializeField] private float lookSpeed;
    public GameObject target;

    public void SetTarget(GameObject _newTarget)
    {
        target = _newTarget;
    }

    private void Update()
    {
        if (!canLookAtTarget) return;
        if (!target) return;
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
    }
}
