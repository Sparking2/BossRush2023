using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentRotator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector3 rotationDirection;
    [SerializeField] private float rotationSpeed = 150.0f;
    private float originalRotationSpeed;
    public bool isCorrectSpeed;
    private void Awake()
    {
        originalRotationSpeed = rotationSpeed;
    }

    public void SetSmoothRotationSpeed(float _newRotation, float _rotationChangeSpeed)
    {
        StartCoroutine(SmoothSpeedChange(_newRotation, _rotationChangeSpeed)); 
    }

    public void ReturnRotationToOriginal()
    {
        StartCoroutine(SmoothSpeedChange(originalRotationSpeed, 550f));
    }

    private IEnumerator SmoothSpeedChange(float _newSpeed,float _rotationChangeSpeed )
    {
        isCorrectSpeed = false;

        if(_newSpeed > rotationSpeed)
        {
            while (rotationSpeed < _newSpeed)
            {
                rotationSpeed += Time.deltaTime * _rotationChangeSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (rotationSpeed > _newSpeed)
            {
                rotationSpeed -= Time.deltaTime * _rotationChangeSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        isCorrectSpeed = true;
        rotationSpeed = _newSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationDirection * (rotationSpeed * Time.deltaTime));
    }
}
