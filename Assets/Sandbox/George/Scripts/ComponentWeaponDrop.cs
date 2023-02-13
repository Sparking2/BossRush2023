using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

public class ComponentWeaponDrop : MonoBehaviour
{
    private DropType dropType;
    [SerializeField] private float movingSpeed;
    [SerializeField] private float startingHeight;
    [SerializeField] private float finalHeight;
    [SerializeField] private LayerMask groundMask;
    private Vector3 finalPosition;

    [SerializeField] private ParticleSystem warningParticles;
    [SerializeField] private GameObject projetileCrate, weaponCrate;

    private FireModeModifier m_fireModifier;
    private ProjectileTypeModifier m_projectile;
    private void Awake()
    {
        dropType = (DropType)Random.Range(0, 2);
        switch (dropType)
        {
            case DropType.projectile:
                m_projectile = gameObject.AddComponent<ProjectileTypeModifier>();
                m_projectile.SetProjectileType(Random.Range(0, 3));
                projetileCrate.SetActive(true);
                break;
            case DropType.weapon:
                m_fireModifier = gameObject.AddComponent<FireModeModifier>();
                m_fireModifier.SetFireMode(Random.Range(0, 4));
                weaponCrate.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, startingHeight, transform.position.z);
        finalPosition = new Vector3(transform.position.x, finalHeight, transform.position.z);
        SetParticlesPosition();

        StartCoroutine(FallingAnimation());
    }

    private IEnumerator FallingAnimation()
    {
        warningParticles.Play();
        while (Vector3.Distance(transform.position, finalPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, finalPosition, Time.deltaTime * movingSpeed);
            yield return null;
            }
        warningParticles.Stop();
        warningParticles.transform.SetParent(transform);
        transform.position = finalPosition;
    }

    private void SetParticlesPosition()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),out hit,Mathf.Infinity, groundMask))
        {
            Vector3 pos = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
            warningParticles.transform.position = pos;
            warningParticles.transform.SetParent(null);
        }
    }

    public enum DropType
    {
        weapon = 0,
        projectile = 1
    }
}
