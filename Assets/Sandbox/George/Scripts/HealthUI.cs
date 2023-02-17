using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance { get; private set; }
    private ComponentHealth playerHealth;
    [SerializeField] private Image healthBar;
    private void Awake()
    {
        Instance = this;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<ComponentHealth>();
    }

    private void Start()
    {
        healthBar.fillAmount = playerHealth.GetHealthValue();
    }

    public void UpdateHealthBar(float value)
    {
        Debug.Log("Bar updated: " + value);

        healthBar.fillAmount = value;
    }
}
