using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHitFeedback : MonoBehaviour
{
    [SerializeField] private int matIndex;
    [SerializeField] private Renderer _renderer;
    private Material material;
    private float value;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        material = _renderer.materials[matIndex];
    }

    public void PerformHitFeedback()
    {
        value = 1.0f;
        material.SetFloat("_HitValue", value);
    }

    private void Update()
    {
        if(value > 0.0f)
        {
            value -= Time.deltaTime * 2.5f;
            material.SetFloat("_HitValue", value);
        } else
        {
            value = 0.0f;
            material.SetFloat("_HitValue", value);
        }
    }
}
