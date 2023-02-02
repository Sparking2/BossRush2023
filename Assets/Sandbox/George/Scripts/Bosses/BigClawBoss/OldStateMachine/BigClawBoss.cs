using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawBoss : BossBase
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootpoint;
    [SerializeField] private BigClawFeet bigBall;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private Transform linePivot;
    [SerializeField] private LineRenderer tackleLineRenderer;

    private void Start()
    {
        tackleLineRenderer.transform.SetParent(transform);
        tackleLineRenderer.enabled = false;
    }

    public void ShootProjectile(int index)
    {
        Instantiate(projectile, shootpoint.position, shootpoint.rotation);
    }

    public override void PerformSpecialAttack()
    {
        bigBall.gameObject.SetActive(true);
        bigBall.OnLaunch(transform.forward);
    }


    public void SetDangerLine()
    {
        if (tackleLineRenderer.transform.parent) tackleLineRenderer.transform.SetParent(null);
        tackleLineRenderer.enabled = true;
        RaycastHit hit;
        tackleLineRenderer.SetPosition(0, linePivot.transform.position);
        if (Physics.Raycast(linePivot.transform.position,transform.TransformDirection(Vector3.forward),out hit,Mathf.Infinity, wallMask))
        {
            Debug.DrawRay(linePivot.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log(hit.transform.name);
            tackleLineRenderer.SetPosition(1, hit.point);
        }
    }

    public void OnTackleFinished()
    {
        tackleLineRenderer.transform.SetParent(transform);
        tackleLineRenderer.enabled = false;
        //RaycastHit hit;
        //tackleLineRenderer.SetPosition(0, transform.position);
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, wallMask))
        //{
        //    tackleLineRenderer.SetPosition(1, hit.point);
        //}
    }

}
