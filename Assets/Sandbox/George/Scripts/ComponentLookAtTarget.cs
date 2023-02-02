using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentLookAtTarget : MonoBehaviour
{
    public bool canLookAtTarget = false;
    [SerializeField] private float lookSpeed;
    private float originalLookSpeed;
    public Vector3 tmpTarget;


    public bool lookAtPlayer = false;
    public Transform playerTransform;

    private void Start()
    {
        originalLookSpeed = lookSpeed;
    }

    public void SetPlayerTransform(Transform _newTarget)
    {
        playerTransform = _newTarget;
    }

    public void SetLookSpeed(float _speed)
    {
        lookSpeed = _speed;
    }

    public void RestoreLookSpeed()
    {
        lookSpeed = originalLookSpeed;
    }

    public void SetTemporalTarget(Vector3 _tmp)
    {
        tmpTarget = _tmp;
        lookAtPlayer = false;
        Invoke("ReturnToLookPlayer", 1.25f);
    }

    private void ReturnToLookPlayer()
    {
        lookAtPlayer = true;
    }

    private void Update()
    {
        if (!canLookAtTarget) return;
        if (lookAtPlayer)
        {
            var lookPos = playerTransform.position - transform.position;
            lookPos.y = 0;
            var targetRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
        } else
        {
            var lookPos = tmpTarget - transform.position;
            lookPos.y = 0;
            var targetRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
        }
    }
}
