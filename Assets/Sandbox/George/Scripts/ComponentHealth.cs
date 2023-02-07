using UnityEngine;

public class ComponentHealth : MonoBehaviour
{

    [Header("Health component: "), Space(10)]
    private float maxHealth;
    [SerializeField] private float health;
    private float bersekerTreshold;
    [SerializeField] private ComponentHealthMaterialController[] healthMaterials;
    private EntityBrainBase m_bossBrain;

    private void Awake()
    {
        healthMaterials = GetComponentsInChildren<ComponentHealthMaterialController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ReduceHealth(Random.Range(5, 25));
        }
    }

    public void SetHealth(float _health, EntityBrainBase brain)
    {
        m_bossBrain = brain;
        maxHealth = _health;
        health = _health;
        bersekerTreshold = health / 2;
        if(healthMaterials.Length >0)
        {
            foreach(var mat in healthMaterials)
            {
                mat.SetHealthValue(1);
            }
        }
    }

    public void ReduceHealth(float _damage)
    {
        health -= _damage;
        if(healthMaterials.Length > 0)
        {
            foreach (var mat in healthMaterials)
            {
                mat.SetHealthValue(health / maxHealth);
            }
        }

        if(health <= bersekerTreshold)
        {
            m_bossBrain.EnterBersekerMode();
        }
    }
}