using Player;
using UnityEngine;

public class ComponentHealth : MonoBehaviour
{

    [Header("Health component: "), Space(10)]
    private float maxHealth;
    [SerializeField] private bool isInvincible;
    [SerializeField] private float health;
    [SerializeField] private GameObject deathParticles;
    private float bersekerTreshold;
    private ComponentHealthMaterialController[] healthMaterials;
    private ComponentInput _componentInput;
    private EntityBrainBase m_bossBrain;
    private ComponentShieldController componentShield;
    private ComponentHitFeedback hitFeedback;
    private void Awake()
    {
        TryGetComponent<ComponentInput>(out _componentInput);
        componentShield = GetComponentInChildren<ComponentShieldController>();
        healthMaterials = GetComponentsInChildren<ComponentHealthMaterialController>();
        hitFeedback = GetComponentInChildren<ComponentHitFeedback>();
    }

    public void SetVulnerability(bool _vulnerable)
    {
        isInvincible = _vulnerable;
    }

    public void SetHealth(float _health, EntityBrainBase brain)
    {
        m_bossBrain = brain;
        maxHealth = _health;
        health = _health;
        bersekerTreshold = health / 2;
        if(healthMaterials.Length > 0)
        {
            foreach(var mat in healthMaterials)
            {
                mat.SetHealthValue(1);
            }
        }
    }

    public void ReduceHealth(float _damage)
    {
        if (componentShield)
            if (componentShield.shieldActive) return;
        if (isInvincible) return;
        if (health <= 0)
        {
            if(m_bossBrain) m_bossBrain.OnDead();
            else
            {
                if (_componentInput)
                {
                    Instantiate(deathParticles, transform.position, Quaternion.identity);
                    _componentInput.enabled = false;
                    gameObject.SetActive(false);
                }
            }
            return;
        }

        if (hitFeedback) hitFeedback.PerformHitFeedback();
        health -= _damage;
        Debug.Log("Getting hit");
        if(healthMaterials.Length > 0)
        {
            foreach (var mat in healthMaterials)
            {
                mat.SetHealthValue(health / maxHealth);
            }
        }

        if (m_bossBrain)
        {
            if (health <= bersekerTreshold && m_bossBrain.isBerseker == false)
            {
                m_bossBrain.EnterBersekerMode();
            }
        }
 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && _componentInput)
        {
            ReduceHealth(10000);
        }
    }
}