using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMoveToTarget : MonoBehaviour
{
    public bool canMove;
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    private Vector3 targetPos;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void SetSpeed(float _speed) { speed = _speed; }

    private void Update()
    {
        if (!canMove) return;
        targetPos = target.position;
        targetPos.y = 1.5f;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
    }
}
