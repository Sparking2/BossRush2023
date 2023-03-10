using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBossLaserGun : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LineRenderer dangerLineRenderer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem preHeatParticles;
    [SerializeField] private ParticleSystem shootLaserParticles;
    [SerializeField] private ParticleSystem shootDecal;
    [Space(10)]
    [SerializeField] private LineRenderer superLaserRenderer;
    [SerializeField] private ParticleSystem superLaserChannelParticles;
    [SerializeField] private ParticleSystem superShootLaserParticles;
    [SerializeField] private ParticleSystem superShootDecal;

    private ComponentDamageOnRay componentDamage;

    private void Awake()
    {
        lineRenderer.enabled = false;
        superLaserRenderer.enabled = false;
        componentDamage = GetComponent<ComponentDamageOnRay>();
    }

    public void SetLaser(bool _active)
    {
        isActive = _active;
        preHeatParticles.Stop();
        dangerLineRenderer.enabled = false;
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

        componentDamage.enabled = _active;
        lineRenderer.enabled = isActive;
    }

    public void PreHeatLaser()
    {
        preHeatParticles.Play();
        dangerLineRenderer.enabled = true;
        dangerLineRenderer.startWidth = 0.1f;
    }

    public void PreHeatSuperLaser()
    {
        superLaserChannelParticles.Play();
        dangerLineRenderer.enabled = true;
        dangerLineRenderer.startWidth = 1f;
    }

    public void SetSuperLaser(bool _active)
    {
        isActive = _active;
        superLaserChannelParticles.Stop();
        dangerLineRenderer.enabled = false;
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

        componentDamage.enabled = _active;
        superLaserRenderer.enabled = isActive;
    }

    private void Update()
    {
        HandlePreHeatDisplay();

        if (!isActive) return;
        HandleLaserShoot();
    }
    RaycastHit _hit;
    RaycastHit hit;
    private void HandlePreHeatDisplay()
    {

        dangerLineRenderer.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _hit, Mathf.Infinity, layerMask))
        {
            dangerLineRenderer.SetPosition(1, _hit.point);
        }
    }

    private void HandleLaserShoot()
    {
        componentDamage.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        dangerLineRenderer.SetPosition(0, transform.position);
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
