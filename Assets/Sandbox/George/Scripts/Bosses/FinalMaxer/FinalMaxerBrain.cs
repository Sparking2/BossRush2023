using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMaxerBrain : EntityBrainBase
{
    // Jumps to move, Move to a place and shoot, Shoot multiple times in different angles, Jump multiple times and make things fall down from the sky.
    // On berseker mode it leaves the ball in the inside and starts doing only melee attacks (Jumping)
    [SerializeField] private float maxAttackTimer;
    [SerializeField] private float minAttackTimer;
    [SerializeField] private bool onAction = false;
    private float c_attackTimer;
    [Header("Jump attack"), Space(10)]
    [SerializeField] private float jumpRange;
    [SerializeField] private int minJumps;
    [SerializeField] private int maxJumps;
    [SerializeField] private ParticleSystem warnCircleParticles;
    [SerializeField] private float warnDuration;
    [SerializeField] private GameObject blastWave;
    [SerializeField] private GameObject fallingObjectPrefab;
    private WaitForSeconds jumpDelay;
    [Header("Laser attack"), Space(10)]
    [SerializeField] private float laserChannelTime;
    [SerializeField] private float laserShootTime;
    [SerializeField] private ParticleSystem laserChargeParticles;
    [SerializeField] private ParticleSystem laserShootParticles;
    [SerializeField] private LineRenderer laserLineRenderer;
    [Space(10)]
    [SerializeField] private ParticleSystem airLaserChargeParticles;
    [SerializeField] private ParticleSystem airLaserShootParticles;
    [SerializeField] private LineRenderer airLaserLineRenderer;

    private bool hasBall = true;
    [SerializeField] private FinalMaxerBall ball;
    [SerializeField] private ComponentShieldController componentShield;
    [SerializeField] private Transform jumpRef;
    private bool animationFinished = false;

    private ComponentLookAtTarget m_componentLookAtTarget;
    private ComponentRotator m_rotator;
    private WaitUntil waitUntilAnimationFinished;

    private FinalBossAnimationCommands animationCommands;
    Vector3 randomPos;
    Vector3 blastWavePosition;
    Vector3 normalizedPlayerPosition;
    public override void OnAwake()
    {
        m_componentLookAtTarget = GetComponent<ComponentLookAtTarget>();
        m_rotator = GetComponent<ComponentRotator>();
        componentShield = GetComponentInChildren<ComponentShieldController>();

        warnDuration = Random.Range(1.5f - 0.25f, 1.5f + 0.25f);

        waitUntilAnimationFinished = new WaitUntil(() => animationFinished == true);
        animationCommands = GetComponentInChildren<FinalBossAnimationCommands>();
        animationCommands.SetBrain(this);

        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    public override void OnStart()
    {
        c_attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
        warnCircleParticles.transform.SetParent(null);
        //componentShield.OnShieldActivation();
        animator.Play("Introduction");
    }

    public override void OnUpdate()
    {

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(jumpRef.position, jumpRange);
    }

    public override void Update()
    {
      //  targetPoint = playerTargetTransform.position;

    }

    

    public override void PerformAction()
    {
        onAction = true;
        animationFinished = false;
        c_attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
        int atk = Random.Range(0, 5);

        switch (atk)
        {
            case 0:
                StartCoroutine(AttackPatternOne());
                break;
            case 1:
                StartCoroutine(AttackPatternTwo());
                break;
            case 2:
                StartCoroutine(AttackPatternThree());
                break;
            case 3:
                StartCoroutine(AttackPatternFour());
                break;
            case 4:
                if(isBerseker) StartCoroutine(AttackPatternFive());
                else StartCoroutine(AttackPatternTwo());
                break;
        }



    }
    // Do jump to player

    // Shoot to the sky and calls down a big projectile or a big laser (BERSEKER)
    private IEnumerator AttackPatternOne() // Performs Jump to the player.
    {
        m_componentLookAtTarget.enabled = false;
        warnDuration = Random.Range(1.5f - 0.25f, 1.5f + 0.25f);
        animationFinished = false;
        int totalJumps = Random.Range(minJumps, maxJumps);
        while(totalJumps > 0)
        {
            totalJumps--;
            yield return new WaitForSeconds(.5f);
            StartCoroutine(Jump(true));
            yield return waitUntilAnimationFinished;
        }

        StartCoroutine(RestingMode());
    }
    private IEnumerator AttackPatternTwo() // Do jump to random spots, in each one shoots a wave.
    {
        m_componentLookAtTarget.enabled = false;
        warnDuration = Random.Range(1.5f - 0.25f, 1.5f + 0.25f);
        animationFinished = false;
        int totalJumps = Random.Range(minJumps, maxJumps);
        int r;
        while (totalJumps > 0)
        {
            totalJumps--;
            r = Random.Range(0, 2);
            yield return new WaitForSeconds(.5f);
            StartCoroutine(Jump(r == 0));
            yield return waitUntilAnimationFinished;
        }
        StartCoroutine(RestingMode());
    }
    private IEnumerator AttackPatternThree()     // Do Jump to the center, lookint to the player, shoots bullet / laser
    {
        m_componentLookAtTarget.enabled = false;
        m_rotator.enabled = false;
        targetPoint = Vector3.zero;
        targetPoint.y = 1f;

        animationFinished = false;
        animator.Play("JumpAtk");
        yield return new WaitForSeconds(1.5f);
        transform.position = targetPoint;
        animator.SetTrigger("warnEnd");
        yield return waitUntilAnimationFinished;
        animationFinished = false;

        if (isBerseker)
        {
            // Jump to the middle and do a laser attack multiple times;\
            animator.Play("LaserStart");
            yield return new WaitForSeconds(laserChannelTime);
            animator.SetTrigger("LaserShot");
            StopChargingLaser();
            ShootLaser();
            m_rotator.enabled = true;
            yield return new WaitForSeconds(laserShootTime);
            animator.SetTrigger("LaserEnd");
            m_rotator.enabled = false;
            StopShootingLaser();
        } else
        {
            m_componentLookAtTarget.enabled = true;
            animator.Play("LaserStart");
            yield return new WaitForSeconds(laserChannelTime);
            animator.SetTrigger("LaserShot");
            StopChargingLaser();
            ShootLaser();
            yield return new WaitForSeconds(laserShootTime);
            animator.SetTrigger("LaserEnd");
            StopShootingLaser();
        }
        yield return new WaitForSeconds(.5f);
        StartCoroutine(RestingMode());
    }
    private IEnumerator AttackPatternFour()    // Jump to the player, shoots, jump, shoots, (x3) 
    {
        int totalAttacks = 3;
        while (totalAttacks > 0)
        {

            animationFinished = false;
            StartCoroutine(Jump(false));
            yield return waitUntilAnimationFinished;
            m_componentLookAtTarget.enabled = true;
            animationFinished = false;
            animator.Play("LaserShot");
            yield return new WaitForSeconds(0.5f);
            m_componentLookAtTarget.enabled = false;
            StopChargingLaser();
            ShootLaser();
            yield return new WaitForSeconds(0.75f);
            animator.SetTrigger("LaserEnd");

            StopShootingLaser();
            totalAttacks--;
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(.5f);
        StartCoroutine(RestingMode());
    }

    private IEnumerator AttackPatternFive()
    {
        animator.Play("AirLaser");
        yield return null;
    }

    public void EnterRestState()
    {
        StartCoroutine(RestingMode());
    }

    private IEnumerator Jump(bool toPlayer)
    {
        animationFinished = false;
        animator.Play("JumpAtk");
        if (toPlayer)
        {
            targetPoint = playerTransform.position;
        } else
        {
            targetPoint = Random.insideUnitSphere * 20f;
        }

        targetPoint.y = 1f;
        yield return new WaitForSeconds(.5f);
        if (warnCircleParticles) warnCircleParticles.Play();
        warnCircleParticles.transform.position = new Vector3(targetPoint.x, targetPoint.y + 0.5f, targetPoint.z);
        yield return new WaitForSeconds(warnDuration);
        transform.position = targetPoint;
        animator.SetTrigger("warnEnd");
    }

    private IEnumerator RestingMode()
    {
        m_rotator.enabled = false;
        m_componentLookAtTarget.enabled = false;
        animator.Play("Rest");
        if (componentShield) componentShield.OnShieldDepleted();
        yield return new WaitForSeconds(restingTime);
        animator.SetTrigger("RestEnd");
        animationFinished = false;
        if (componentShield) componentShield.OnShieldActivation();
        yield return waitUntilAnimationFinished;
        PerformAction();
    }

    public void OnBersekerStart()
    {
        if (componentShield) componentShield.OnShieldActivation();
        PerformAction();
        if (componentHealth) componentHealth.SetVulnerability(false);
    }

    public void ChargeLaser()
    {
        laserChargeParticles.Play();
    }
    private void StopChargingLaser()
    {
        laserChargeParticles.Stop();
    }
    private void ShootLaser()
    {
        laserShootParticles.Play();
        laserLineRenderer.enabled = true;
    }
    private void StopShootingLaser()
    {
        laserShootParticles.Stop();
        laserLineRenderer.enabled = false;
    }

    public void ChargeAirLaser()
    {
        airLaserChargeParticles.Play();
    }

    public void ShootAirLaser()
    {
        airLaserChargeParticles.Stop();
        airLaserShootParticles.Play();
        airLaserLineRenderer.enabled = true;
    }

    public void StopAirLaser()
    {
        airLaserShootParticles.Stop();
        airLaserLineRenderer.enabled = false;
    }

    public void OnImpact()
    {
        if (warnCircleParticles) warnCircleParticles.Stop();
        randomPos = Random.insideUnitSphere * 15.0f;
        blastWavePosition = transform.position;
        randomPos.y = 1.1f;

        blastWavePosition.y = 1.5f;
        Instantiate(blastWave, blastWavePosition, Quaternion.identity);
        int fallingObjects = Random.Range(3, 9);
        normalizedPlayerPosition = playerPos;
        normalizedPlayerPosition.y = 1.1f;
        Instantiate(fallingObjectPrefab, normalizedPlayerPosition, Quaternion.identity);
        for (int i =0;i < fallingObjects; i++)
        {
            randomPos = Random.insideUnitSphere * 15.0f;
            randomPos.y = 1.1f;
            if (randomPos == Vector3.positiveInfinity || randomPos == Vector3.negativeInfinity) randomPos = playerPos;
            Instantiate(fallingObjectPrefab, randomPos, Quaternion.identity);
        }
    }

    public void SelectRandomPoint()
    {
        targetPoint = CustomTools.GetRandomPointOnMesh(15f, Vector3.zero);
        transform.position = targetPoint;
        if (warnCircleParticles) warnCircleParticles.Play();
    }

    public void OnAnimationFinished()
    {
        animationFinished = true;
    }

    public override void EnterBersekerMode()
    {
        base.EnterBersekerMode();
        if (componentShield) componentShield.OnShieldDepleted();
        if (componentHealth) componentHealth.SetVulnerability(true);
    }
}
