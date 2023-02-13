using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentShieldController : MonoBehaviour
{
    public bool shieldActive = false;
    private Renderer m_Renderer;
    private Material shieldMaterial;

    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
        shieldMaterial = m_Renderer.material;
    }

    public void OnShieldDepleted()
    {
        shieldActive = false;
        shieldMaterial.SetFloat("_ShieldPower", 0f);
    }

    public void OnShieldActivation()
    {
        shieldActive = true;
        shieldMaterial.SetFloat("_ShieldPower",1f);
    }
}
