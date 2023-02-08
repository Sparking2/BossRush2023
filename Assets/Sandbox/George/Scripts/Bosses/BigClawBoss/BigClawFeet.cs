using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawFeet : MonoBehaviour
{
    [SerializeField] private float launchSpeed;
    private Transform playerTransform;
    private Rigidbody _rigidbody;
    [SerializeField] private Transform origin;
    [SerializeField] private LineRenderer chainLineRenderer;
    private BigClawBrain brain;
    private void Awake()
    {
        brain = GetComponentInParent<BigClawBrain>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Launch()
    {
        gameObject.SetActive(true);
        transform.localScale = new Vector3(0.75f, 0.75f, 1.75f);
        chainLineRenderer.enabled = true;
        Vector3 dir = playerTransform.position - transform.position;
        dir.y += 2.0f;
        _rigidbody.velocity = dir * launchSpeed;
    }

    private void Update()
    {
        chainLineRenderer.SetPosition(0, origin.transform.position);
        chainLineRenderer.SetPosition(1, transform.position);
    }

    public void ReturnToOrigin()
    {
        StartCoroutine(ReturnTween());
    }

    private IEnumerator ReturnTween()
    {
        transform.localScale = new Vector3(0.75f, 0.75f, 1.75f);
        transform.LookAt(origin);
        while(Vector3.Distance(transform.position,origin.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, origin.position, 55.0f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.position = origin.position;
        gameObject.SetActive(false);
        brain.OnFeetReturned();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            transform.localScale = Vector3.one;
            _rigidbody.velocity = Vector3.zero;
        }
    }
}
