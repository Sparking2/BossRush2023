using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EntityBrainBase : MonoBehaviour
{
    [Header("Boss base paremeters: ") ,Space(10)]
    public EntityState state = EntityState.idle;
    public int maxHealth;
    public float idleMovementSpeed;
    public bool isBerseker;
    public float restingTime;


    [HideInInspector] public float attackTimer;
    [SerializeField] private bool hasMovingAnimation;
    [HideInInspector] public bool canDoAction = true;
    private float originalAcceleration;

    
    [HideInInspector] public Vector3 targetPoint;
     public Transform playerTransform;
    [HideInInspector] public Transform playerTargetTransform;
    [HideInInspector] public Vector3 playerPos;
    [HideInInspector] public WaitUntil waitUntilIsOnTarget;

    public ComponentHealth componentHealth;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    [SerializeField] private ParticleSystem berserkerChannelParticles;
    [SerializeField] private ParticleSystem berserkerBurstParticles;

    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private ParticleSystem deathExplotion;
    private CameraControl _cameraControl;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        componentHealth = GetComponent<ComponentHealth>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerTargetTransform = playerTransform.Find("PlayerTarget").GetComponent<Transform>();

        waitUntilIsOnTarget = new WaitUntil(() => HandleTargetCheck(targetPoint));
        _cameraControl = GetComponent<CameraControl>();
        OnAwake();
    }

    private void Start()
    {
        targetPoint = playerTransform.position;
        if(agent) originalAcceleration = agent.acceleration;
        canDoAction = true;
        if (agent) SetBaseAgentSettings();
        if (componentHealth) componentHealth.SetHealth(maxHealth, this);


        OnStart();
        //PerformAction();
    }
    public void BringCamera()
    {
        _cameraControl.StartBlend();
    }

    public void ReturnCameraToPlayer()
    {
        _cameraControl.ReturnToPlayer();
    }
    public void SetBaseAgentSettings()
    {
        agent.speed = idleMovementSpeed;
        agent.acceleration = originalAcceleration;
    }

    public virtual void Update() // Basic profile for any boss, can be overrriten to do a better one
    {
        playerPos = new Vector3(playerTransform.position.x, playerTransform.position.y + 1.5f, playerTransform.position.z);
        OnUpdate();
    }

    public void OnActionFinished()
    {
        SetBaseAgentSettings();
        OnRestingStart();
        componentHealth.SetVulnerability(false);
    }
 
    public virtual IEnumerator MoveToRandomPoint()
    {
        state = EntityState.moving;
        targetPoint = CustomTools.GetRandomPointOnMesh(15f,Vector3.zero);
        if (hasMovingAnimation) animator.SetBool("isMoving", true);
        if(agent) agent.SetDestination(targetPoint);
        yield return waitUntilIsOnTarget;
        animator.SetBool("isMoving", false);
        OnActionFinished();
    }

    private bool HandleTargetCheck(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) <= 0.5;
    }

    public virtual void EnterBersekerMode()
    {
        if (isBerseker) return;
        isBerseker = true;
        LightManager.Instance.TurnOffTheLights(true);
        StopAllCoroutines();
        CancelInvoke("OnRestingEnd");
        if (agent) agent.SetDestination(transform.position);
        state = EntityState.idle;
        componentHealth.SetVulnerability(true);

        animator.Play("BersekerEnter");
    }

    private void OnRestingStart()
    {
        state = EntityState.resting;     
        Invoke("OnRestingEnd", restingTime);
    }

    private void OnRestingEnd()
    {
        state = EntityState.idle;
        PerformAction();
    }

    public void PlayBerserkerChannelVFX()
    {
        berserkerChannelParticles.Play();
    }

    public void PlayBersekerBurstVFX()
    {
        berserkerBurstParticles.Play();
        berserkerChannelParticles.Stop();
        LightManager.Instance.TurnOnTheLights(true);
    }

    public void OnDead()
    {
        state = EntityState.dead;
        CancelInvoke("OnRestingEnd");
        if (agent) agent.SetDestination(transform.position);
        StopAllCoroutines();
        StartCoroutine(DeathAnimation());
    }



    private IEnumerator DeathAnimation()
    {
        LightManager.Instance.TurnOffTheLights(true);
        animator.Play("Death");
        deathParticles.Play();
        yield return new WaitForSeconds(3.5f);
        deathExplotion.Play();
        yield return new WaitForSeconds(.45f);
        gameObject.SetActive(false);
        _cameraControl.ReturnToPlayer();
        BossManager.Instance.OnBossKilled();
        LightManager.Instance.TurnOnTheLights(true);
    }

    #region Abstracts methods
    public abstract void OnAwake();
    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void PerformAction();    // Does an action, can be moving, channeling, attacking, all of this is made in the actuall entity boss script 

    #endregion
    
    public enum EntityState { idle, channeling, attacking,moving,resting,dead}
}
