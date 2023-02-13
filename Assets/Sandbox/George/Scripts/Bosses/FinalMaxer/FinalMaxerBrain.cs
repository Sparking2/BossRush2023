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
    [Header("Jump attack"),Space(10)]
    [SerializeField] private ParticleSystem warnCircleParticles;
    [SerializeField] private float warnDuration;
    [SerializeField] private GameObject blastWave;
    [SerializeField] private GameObject fallingObjectPrefab;
    private WaitForSeconds jumpDelay;
    [Header("Laser attack"), Space(10)]
    [SerializeField] private float laserChannelTime;
    [SerializeField] private ParticleSystem laserChargeParticles;
    [SerializeField] private ParticleSystem laserShootParticles;
    [SerializeField] private LineRenderer laserLineRenderer;

     private bool hasBall = true;
    [SerializeField] private FinalMaxerBall ball;
    [SerializeField] private ComponentShieldController componentShield;

    private bool animationFinished = false;

    private ComponentLookAtTarget m_componentLookAtTarget;
    private WaitUntil waitUntilAnimationFinished;

    public override void Update()
    {
        if (onAction) return;
        if (c_attackTimer > 0f) c_attackTimer -= Time.deltaTime;
        else
        {
            PerformAction();
        }
    }

    private IEnumerator JumpAttack(bool _singleJump)
    {
        m_componentLookAtTarget.lookAtPlayer = false;
        warnDuration = Random.Range(1.5f - 0.25f, 1.5f + 0.25f);
        if (_singleJump)
        {
            yield return new WaitForSeconds(.5f);
            animator.Play("JumpAtk");
            yield return new WaitForSeconds(warnDuration);
            animator.SetTrigger("warnEnd");
        } else
        {
            int totalJumps = Random.Range(2,5);
            while(totalJumps > 0)
            {
                animationFinished = false;
                yield return new WaitForSeconds(.15f);
                animator.Play("JumpAtk");
                yield return new WaitForSeconds(warnDuration);
                animator.SetTrigger("warnEnd");
                totalJumps--;
                yield return waitUntilAnimationFinished;
            }
        }
        onAction = false;
        m_componentLookAtTarget.lookAtPlayer = true;
        componentShield.OnShieldDepleted();
    }

    private IEnumerator LaserAttack()
    {
        m_componentLookAtTarget.canLookAtTarget = true;
        ChargeLaser();
        yield return new WaitForSeconds(laserChannelTime);
        m_componentLookAtTarget.canLookAtTarget = false;
        StopChargingLaser();
        ShootLaser();
        yield return new WaitForSeconds(1.0f);
        StopShootingLaser();
        componentShield.OnShieldDepleted();
        onAction = false;
    }

    private void ChargeLaser()
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

    public void OnImpact()
    {
        if (warnCircleParticles) warnCircleParticles.Stop();
        Instantiate(blastWave, warnCircleParticles.transform.position,Quaternion.identity);
        int fallingObjects = Random.Range(3, 9);
        Instantiate(fallingObjectPrefab, playerPos, Quaternion.identity);
        for (int i =0;i < fallingObjects; i++)
        {
            Vector3 randomPos = CustomTools.GetRandomPointOnMesh(15.0f, Vector3.zero);
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

    public void JumpOverPlayer()
    {
        transform.position = playerTransform.position;
        if (warnCircleParticles) warnCircleParticles.Play();
    }

    public void OnAnimationFinished()
    {
        animationFinished = true;
    }

    public override void OnAwake()
    {
        m_componentLookAtTarget = GetComponent<ComponentLookAtTarget>();
        componentShield = GetComponentInChildren<ComponentShieldController>();

        warnDuration = Random.Range(1.5f - 0.25f, 1.5f + 0.25f);

        waitUntilAnimationFinished = new WaitUntil(() => animationFinished == true);
    }

    public override void OnStart()
    {
        c_attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
        componentShield.OnShieldActivation();
    }

    public override void OnUpdate()
    {

    }

    public override void PerformAction()
    {
        onAction = true;
        animationFinished = false;
        c_attackTimer = Random.Range(minAttackTimer, maxAttackTimer);
        int atk = Random.Range(0,2);
        componentShield.OnShieldActivation();

        if (hasBall == false)
        {
            StartCoroutine(JumpAttack(true));
            return;
        }
        switch (atk)
        {
            case 0:
                StartCoroutine(JumpAttack(Random.Range(0, 2) == 1));
                break;
            case 1:
                StartCoroutine(LaserAttack());
                break;
        }
    }

    private void ReleaseTheBall()
    {
        // Do animation here
        ball.OnRelease();
    }

    public override void EnterBersekerMode()
    {
        base.EnterBersekerMode();
        ReleaseTheBall();
    }
}
