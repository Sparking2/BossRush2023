using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMaxerBall : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private float length;
    [SerializeField] private float ballSpeed;
    private float timeCounter;

    float x;
    float y;
    float z;

    [SerializeField] private Transform ballTransform;

    private void Start()
    {
        y = height;
    }

    private void Update()
    {
            timeCounter += Time.deltaTime * ballSpeed;

        x = Mathf.Cos(timeCounter) * width;
        y = 2f;
        z = Mathf.Sin(timeCounter) * length;

        Vector3 movingPos = new Vector3(x, y, z);

        transform.position = Vector3.MoveTowards(transform.position,movingPos,15 * Time.deltaTime);
    }

    public void OnRelease()
    {
        gameObject.SetActive(true);
        transform.SetParent(null);
    }

    private void OnEnable()
    {
        transform.SetParent(null);
    }

}
