using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBossLaserGun : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem preHeatParticles;
    [SerializeField] private ParticleSystem shootLaserParticles;
    [SerializeField] private ParticleSystem shootDecal;
    [Space(10)]
    [SerializeField] private LineRenderer superLaserRenderer;
    [SerializeField] private ParticleSystem superLaserChannelParticles;
    [SerializeField] private ParticleSystem superShootLaserParticles;
    [SerializeField] private ParticleSystem superShootDecal;

    private void Awake()
    {
        lineRenderer.enabled = false;
        superLaserRenderer.enabled = false;
    }

    public void SetLaser(bool _active)
    {
        isActive = _active;
        preHeatParticles.Stop();
        if (isActive)
        {
            shootLaserParticles.Play();
            shootDecal.Play();
        }
        else
        {
            shootLaserParticles.Stop();
            shootDecal.Stop();
        }

        lineRenderer.enabled = isActive;
    }

    public void PreHeatLaser()
    {
        preHeatParticles.Play();
    }

    public void PreHeatSuperLaser()
    {
        superLaserChannelParticles.Play();
    }

    public void SetSuperLaser(bool _active)
    {
        isActive = _active;
        superLaserChannelParticles.Stop();

        if (isActive)
        {
            superShootLaserParticles.Play();
            superShootDecal.Play();
        }
        else
        {
            superShootLaserParticles.Stop();
            superShootDecal.Stop();
        }

        
        superLaserRenderer.enabled = isActive;
    }

    private void Update()
    {
        if (!isActive) return;
        HandleLaserShoot();
    }


    private void HandleLaserShoot()
    {
        RaycastHit hit;
        lineRenderer.SetPosition(0, transform.position);
        superLaserRenderer.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            lineRenderer.SetPosition(1, hit.point);
            superLaserRenderer.SetPosition(1, hit.point);
            shootDecal.transform.position = hit.point;
            superShootDecal.transform.position = hit.point;
        }
    }
}
