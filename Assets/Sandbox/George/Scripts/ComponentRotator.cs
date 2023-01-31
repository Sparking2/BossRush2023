using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentRotator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector3 rotationDirection;
    [SerializeField] private float rotationSpeed = 150.0f;

    public void SetRotationSpeed(float _newRotation)
    {
        rotationSpeed = _newRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationDirection * (rotationSpeed * Time.deltaTime));
    }
}
