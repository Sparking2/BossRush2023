using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHealthMaterialController : MonoBehaviour
{
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;

    }

    public void SetHealthValue(float _value)
    {
        material.SetFloat("_health", _value);
    }
}
