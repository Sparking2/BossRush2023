using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawFeet : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 15;
    private bool ballHit = false;
    [SerializeField] private float retractCooldown = 3.0f;
    [SerializeField] private Transform initialPosition;
    private Rigidbody rb;
    [SerializeField] private LineRenderer lineRenderer;

    public delegate void OnBallCaptured();
    public static event OnBallCaptured OnCapture;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        transform.SetParent(null);
    }
    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        // LaunchBall here
        lineRenderer.enabled = true;

        retractCooldown = 1.0f;
        ballHit = false;
    }

    public void OnLaunch(Vector3 _launchDir)
    {
        transform.position = initialPosition.position;

        rb.velocity = _launchDir * launchSpeed;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, initialPosition.position);

    }

    private void ReturnToBoss()
    {
        Vector3 dir = initialPosition.position - transform.position;
        rb.velocity = dir * 3.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") && !ballHit)
        {
            rb.velocity = Vector3.zero;
            ballHit = true;
            Invoke("ReturnToBoss", retractCooldown);
        } else if(other.CompareTag("Boss") && ballHit)
        {
            rb.velocity = Vector3.zero;
            ballHit = false;

            gameObject.SetActive(false);
            OnCapture?.Invoke();
        }
    }
}
