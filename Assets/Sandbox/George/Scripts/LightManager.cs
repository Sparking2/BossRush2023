using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance { get; private set; }
    private float directionalLightIntensity;
    private float spothLightsIntensity;
    [SerializeField] private float directionalSmooth;
    [SerializeField] private float spothlightSmooth;
    [SerializeField] private Light[] _ligths;

    private void Awake()
    {
        Instance = this;
        _ligths = GetComponentsInChildren<Light>();
    }

    private void Start()
    {
        directionalLightIntensity = _ligths[0].intensity;
        spothLightsIntensity = _ligths[1].intensity;
    }

    public void TurnOffTheLights(bool smooth)
    {
        if (smooth)
        {
            // Smooth corutine
            StartCoroutine(ChangeDirectionalIntensity(true));
            StartCoroutine(ChangeSpothlightsIntensity(true));
        } else
        {
            foreach(Light light in _ligths)
            {
                light.intensity = 0f;
            }
        }

    }
    private IEnumerator ChangeDirectionalIntensity(bool _off)
    {
        float _intensity = _ligths[0].intensity;
        if (_off)
        {
            while(_intensity > 0f)
            {
                _intensity -= Time.deltaTime * directionalSmooth;
                _ligths[0].intensity = _intensity;
                yield return new WaitForEndOfFrame();
            }
        } else
        {
            while (_intensity < 1f)
            {
                _intensity += Time.deltaTime * directionalSmooth;
                _ligths[0].intensity = _intensity;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private IEnumerator ChangeSpothlightsIntensity(bool _off)
    {
        float _intensity = _ligths[1].intensity;
        if (_off)
        {
            while (_intensity > 0f)
            {
                _intensity -= Time.deltaTime * directionalSmooth;
                _ligths[1].intensity = _intensity;
                _ligths[2].intensity = _intensity;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (_intensity < spothLightsIntensity)
            {
                _intensity += Time.deltaTime * spothlightSmooth;
                _ligths[1].intensity = _intensity;
                _ligths[2].intensity = _intensity;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void TurnOnTheLights(bool smooth)
    {
        if (smooth)
        {
            StartCoroutine(ChangeDirectionalIntensity(false));
            StartCoroutine(ChangeSpothlightsIntensity(false));
        } else
        {
            for(int i = 0; i < _ligths.Length; i++)
            {
                if (i == 0) _ligths[i].intensity = directionalLightIntensity;
                else _ligths[i].intensity = spothLightsIntensity;
            }
        }
    }
}
