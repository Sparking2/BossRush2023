using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private bool canShootProjectiles;
    [SerializeField] private Transform[] shootpoints;
    [SerializeField] private GameObject projectilePrefab;
    [Space(10)]
    [Header("In and out parameters: "),Space(10)]
    [SerializeField] private float movementSpeed = 15.0f;
    [SerializeField] private float stayingDuration = 1.5f;
    [SerializeField] private float returningSpeed = 7.5f;

    [Header("Bullet vortex parameters: "), Space(10)]
    [SerializeField] private float vortexSpeed;
    [SerializeField] private float vortexWidth;
    [SerializeField] private float vortexHeight;
    [Space(10)]
    [SerializeField] private float shootingSpeed = 0.5f;


    private WaitForSeconds waitDuration;
    float shootCounter;
    private Vector3 target;
    private Transform bossPosition;

    private Vector3 velocity;
    private BoomerangBoss m_boss;

    private void Start()
    {
        waitDuration = new WaitForSeconds(stayingDuration);
        shootCounter = shootingSpeed;
    }

    public void SetBoomerangBoss(BoomerangBoss _boss) { m_boss = _boss; }

    public void SetBossPosition(Transform _position) { bossPosition = _position; }


    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (canShootProjectiles == false) return;
        if (shootCounter > 0.0f) shootCounter -= Time.deltaTime;
        else
        {
            shootCounter = shootingSpeed;
            foreach (Transform point in shootpoints)
            {
                Instantiate(projectilePrefab, point.position, point.rotation);
            }
        }
    }
    // Go to the last position of the player
    public void PerformInAndOutAttack(Vector3 _target)
    {
        OnLaunch();
        target = _target;
        StartCoroutine(InAndOutAttack());
    }

    private void OnLaunch()
    {
        transform.SetParent(null);
        gameObject.SetActive(true);

    }

    private IEnumerator InAndOutAttack()
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, movementSpeed);

            yield return new WaitForEndOfFrame();
        }
        transform.position = target;

        yield return waitDuration;
        canShootProjectiles = false;
        while (Vector3.Distance(transform.position, bossPosition.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, bossPosition.position, returningSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        OnBoomerangReturned();
        yield return null;
    }

    private void OnBoomerangReturned()
    {
        transform.position = bossPosition.position;
        gameObject.SetActive(false);
        transform.SetParent(bossPosition.transform);
        m_boss.OnBoomerangReturn();
    }

    // Perform an attack to the middle of the arena and shoot a lot of projectiles.
    public void PerformBulletVortexAttack()
    {
        OnLaunch();
        StartCoroutine(BulletVortextAttack());

    }

    private IEnumerator BulletVortextAttack()
    {
        target = new Vector3(0, 1.75f, 0);
        
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, movementSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.position = target;
        float timer = 15.0f;
        float timeCounter = 0f;

        float cWidth = 0;
        float cHeight = 0;

        canShootProjectiles = true;
        while( timer > 0.0f)
        {
            timeCounter += Time.deltaTime * vortexSpeed;
            timer -= Time.deltaTime;

            float x = Mathf.Cos(timeCounter) * cWidth;
            float y = 1.75f;
            float z = Mathf.Sin(timeCounter) * cHeight;

            if (cWidth < vortexWidth) cWidth += Time.deltaTime * 1.5f;
            else cWidth = vortexWidth;

            if (cHeight < vortexHeight) cHeight += Time.deltaTime * 1.5f;
            else cHeight = vortexHeight;

            transform.position = new Vector3(x, y, z);
            yield return new WaitForEndOfFrame();
        }
        canShootProjectiles = false;

        yield return new WaitForSeconds(0.5f);

        while (Vector3.Distance(transform.position, bossPosition.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, bossPosition.position, returningSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        OnBoomerangReturned();
    }

    // Chase the player and explode
    public void PerformExplosiveAttack()
    {

    }
}
