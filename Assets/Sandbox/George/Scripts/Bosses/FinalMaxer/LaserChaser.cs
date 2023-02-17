using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserChaser : MonoBehaviour
{
    [SerializeField] private ParticleSystem warnParticles;
    [SerializeField] private ParticleSystem laserParticles;
    [SerializeField] private GameObject laser;
    [SerializeField] private ParticleSystem afterParticles;
    [SerializeField] private Collider _collider;
    private ComponentMoveToTarget moveToTarget;

    private void Awake()
    {
        moveToTarget = GetComponent<ComponentMoveToTarget>();
    }

    private void Start()
    {
        ActivateLaser();
    }

    public void SetLaser(float speed)
    {
        moveToTarget.SetSpeed(speed);
    }

    public void ActivateLaser()
    {
        gameObject.SetActive(true);
        warnParticles.Play();
        Invoke("PerformLaser", 4.5f);
        moveToTarget.canMove = true;
    }

    private void PerformLaser()
    {
        warnParticles.Stop();
        laserParticles.Play();
        laser.SetActive(true);
        Invoke("StopLaser", 5.5f);
    }

    public void StopLaser()
    {
        moveToTarget.canMove = false;
        laserParticles.Stop();
        laser.SetActive(false);
        afterParticles.Play();
        _collider.enabled = false;
        Destroy(gameObject, 1.5f);
    }
}
