using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [Space(10)]
    [Header("Boomerang parameters: "),Space(10)]
    [SerializeField] private float launchSpeed;
    [SerializeField] private float stayingDuration = 1.5f;
    [SerializeField] private float returningSpeed = 7.5f;
    [SerializeField] private float incrementalRotationSpeed;
    [SerializeField] private float shootingSpeed = 0.5f;

    [Header("Bullet vortex parameters: "), Space(10)]

    [SerializeField] private float vortexDuration;
    [SerializeField] private float vortexSpeed;
    [SerializeField] private float vortexWidth;
    [SerializeField] private float vortexHeight;
    [SerializeField] private float vortexGrowth = 1.5f;
    [SerializeField] private ComponentIndependantGun[] independantGuns;
    [Space(10)]

    [Header("Boomerang explotion parameters: "), Space(10)]
    [SerializeField] private float initialMovementSpeed;
    [SerializeField] private float speedDecay;
    [SerializeField] private GameObject explotionPrefab;
    [SerializeField] private ComponentBossLaserGun[] bossLaserGuns;

    [Header("Boomerang laser paraemeters: "), Space(10)]
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float laserDuration;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float superLaserDuration;
    [SerializeField] private float superLaserChannelTime;
    [SerializeField] private float superLaserRotationSpeed;



    float shootCounter;
    private Vector3 target;
    private Transform bossPosition;

    private Vector3 velocity;
    private BoomerangBoss m_boss;
    private ComponentRotator m_rotator;

    private WaitForSeconds waitDuration;
    private WaitForSeconds laserWaitDuration;
    private WaitForSeconds superLaserWaitDuration;
    private WaitForSeconds superLaserWaitChannelTime;

    private WaitForEndOfFrame OnFrameEnd;

    private WaitUntil waitUntilArriveToTarget;
    private WaitUntil waitUntilRotationIsFinished;

    float timer;
    float timeCounter = 0f;

    float cWidth = 0;
    float cHeight = 0;

    float x;
    float y;
    float z;
    private void Awake()
    {
        waitDuration = new WaitForSeconds(stayingDuration);
        laserWaitDuration = new WaitForSeconds(laserDuration);
        superLaserWaitChannelTime = new WaitForSeconds(superLaserChannelTime);
        superLaserWaitDuration = new WaitForSeconds(superLaserDuration);
        waitUntilArriveToTarget = new WaitUntil(() => isInTarget == true);
        waitUntilRotationIsFinished = new WaitUntil(() => m_rotator.isCorrectSpeed == true);
        OnFrameEnd = new WaitForEndOfFrame();
        shootCounter = shootingSpeed;
        isInTarget = false;

        m_rotator = GetComponent<ComponentRotator>();
    }

    private void Start()
    {
        for(int i=0;i < independantGuns.Length; i++)
        {
            independantGuns[i].SetShootSpeed(shootingSpeed);
        }
    }

    public void SetBoomerangBoss(BoomerangBoss _boss) { m_boss = _boss; }

    public void SetBossPosition(Transform _position) { bossPosition = _position; }

    public void SetBersekerMode(bool b)
    {
        foreach(ComponentIndependantGun gun in independantGuns)
        {
            gun.SetBersekerState(b);
        }
    }

    private void SetGuns(int pattern)
    {
        if (independantGuns.Length == 0) return;
        switch (pattern)
        {
            case 0: // 0 and 2
                independantGuns[0].SetGunState(true);
                independantGuns[2].SetGunState(true);
                break;
            case 1: // All
                foreach(ComponentIndependantGun gun in independantGuns)
                {
                    gun.SetGunState(true);
                }
                break;
        }
    }

    private void SetIndividualGun(int gunIndex)
    {
        independantGuns[gunIndex].SetGunState(true);
    }

    //private void ChangeGunType(int _gunIndex,ComponentIndependantGun.GunStyle newStyle)
    //{
    //    // Change a gun to a new type of shooting
    //  //  independantGuns[_gunIndex].SetShootingMode(newStyle);
    //}

    private void StopGuns()
    {
        foreach(ComponentIndependantGun gun in independantGuns)
        {
            gun.SetGunState(false);
        }
    }
    private void SetLasers(bool _state)
    {
        foreach (ComponentBossLaserGun laserGun in bossLaserGuns)
        {
            laserGun.SetLaser(_state);
        }
    }

    private void SetSuperLasers(bool _state)
    {
        foreach (ComponentBossLaserGun laserGun in bossLaserGuns)
        {
            laserGun.SetSuperLaser(_state);
        }
    }

    private void SetDualLaser(bool _state)
    {
        bossLaserGuns[1].SetLaser(_state);
        bossLaserGuns[3].SetLaser(_state);
    }
    public void OnBoomerangLaunch()
    {
        transform.SetParent(null);
        gameObject.SetActive(true);

    }

    public void OnBoomerangReturned()
    {
        transform.position = bossPosition.position;
        gameObject.SetActive(false);
        transform.SetParent(bossPosition.transform);
        m_boss.OnBoomerangReturn();
    }

    #region ATTACK PATTERNS
    // Go to the last position of the player
    public void PerformInAndOutAttack(Vector3 _target,bool bersekerMode)
    {
       // OnBoomerangLaunch();
 
        StartCoroutine(InAndOutAttack(_target, bersekerMode));
    }

    private IEnumerator InAndOutAttack(Vector3 _target,bool _berseker) // In besecker mode it shoots from 2 sides while chasing.
    {
        StartCoroutine(MoveToTarget(_target, launchSpeed));
        if (_berseker)
        {
            SetGuns(0);
        }
        yield return waitUntilArriveToTarget;
        yield return waitDuration;
        StopGuns();
        StartCoroutine(ReturnToBoss());
    }

    // Perform an attack to the middle of the arena and shoot a lot of projectiles.
    public void PerformBulletVortexAttack(bool _berseker)
    {
      //  OnBoomerangLaunch();
        StartCoroutine(BulletVortextAttack(_berseker));
    }

    private IEnumerator BulletVortextAttack(bool _berseker) // In berseker mode there is a chance to shoot bigger projectiles
    {
        target = new Vector3(0f,1.25f,0f);
        StartCoroutine(MoveToTarget(target, launchSpeed));

        yield return waitUntilArriveToTarget;
    
        yield return waitDuration;
        SetGuns(1);
        timer = vortexDuration;
        y = target.y;
     

        while ( timer > 0.0f) // Move in a circle 
        {
            timeCounter += Time.deltaTime * vortexSpeed;
            timer -= Time.deltaTime;

            x = Mathf.Cos(timeCounter) * cWidth;      
            z = Mathf.Sin(timeCounter) * cHeight;

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, y, z), vortexSpeed * Time.deltaTime);
            if (cWidth < vortexWidth) cWidth += Time.deltaTime * vortexGrowth;
            else cWidth = vortexWidth;

            if (cHeight < vortexHeight) cHeight += Time.deltaTime * vortexGrowth;
            else cHeight = vortexHeight;


            yield return OnFrameEnd;
        }
        StopGuns();
        cWidth = 0;
        cHeight = 0;
        yield return waitDuration;

        StartCoroutine(ReturnToBoss());
    }

    // Chase the player and explode
    public void PerformExplosiveAttack(Transform _target)
    {
        StartCoroutine(ExplosiveAttack(_target));
    }

    private IEnumerator ExplosiveAttack(Transform _target)
    {
        // In bersecker mode the boomerang explode and shoot multiple projectiles, also chases the player another time.
        target = _target.position;
        float speed = initialMovementSpeed;
        while (speed > 0.0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
            speed -= Time.deltaTime * speedDecay;
            if (Vector3.Distance(transform.position, _target.position) < 0.1f) break;
            yield return OnFrameEnd;
        }
        speed = 0f;
        Instantiate(explotionPrefab, transform.position, transform.rotation);
        StartCoroutine(ReturnToBoss());
    }

    public void PerformLaserAttack(Vector3 _targetPosition, bool _isBerseker)
    {
        StartCoroutine(LaserAttack(_targetPosition, _isBerseker));
    }


    private IEnumerator LaserAttack(Vector3 _targetPosition,bool _isBerseker)
    {
        // In bersecker mode the laser does a final laser burst in a bigger size
        target = _targetPosition;
        target.y = 0.6f;
        StartCoroutine(MoveToTarget(target, launchSpeed));
        yield return waitUntilArriveToTarget;

        foreach (ComponentBossLaserGun laserGun in bossLaserGuns)
        {
            laserGun.PreHeatLaser();
        }

        m_rotator.SetSmoothRotationSpeed(rotationSpeed, incrementalRotationSpeed);
        yield return waitUntilRotationIsFinished;
        SetLasers(true);
        yield return laserWaitDuration;
        SetLasers(false);
        if (_isBerseker)
        {
            foreach (ComponentBossLaserGun laserGun in bossLaserGuns)
            {
                laserGun.PreHeatSuperLaser();
            }
            m_rotator.SetSmoothRotationSpeed(superLaserRotationSpeed, incrementalRotationSpeed/2f);
            yield return superLaserWaitChannelTime;
            SetSuperLasers(true);
            yield return superLaserWaitDuration;
        }
        SetSuperLasers(false);
        m_rotator.ReturnRotationToOriginal();
        yield return waitUntilRotationIsFinished;
        StartCoroutine(ReturnToBoss());
        // Channel code here
    }



    private bool isInTarget;

    private IEnumerator MoveToTarget(Vector3 _target, float _speed)
    {
        isInTarget = false;
        while (Vector3.Distance(transform.position, _target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
            yield return OnFrameEnd;
        }
        transform.position = _target;
        isInTarget = true;
    }

    private IEnumerator ReturnToBoss()
    {
        while (Vector3.Distance(transform.position, bossPosition.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, bossPosition.position, returningSpeed * Time.deltaTime);
            yield return OnFrameEnd;
        }
        OnBoomerangReturned();
    }
    #endregion
}
