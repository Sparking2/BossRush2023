using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWave : MonoBehaviour
{
    public int startWidth;
    public int pointsCount;
    public float maxRadius;
    public float speed;

    private LineRenderer _lineRenderer;
    private MeshCollider _collider;
    private Mesh _mesh;
    private void Awake()
    {
        _collider = GetComponent<MeshCollider>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = pointsCount + 1;
        _mesh = new Mesh();
        if (_collider == null)
        {
            _collider = gameObject.AddComponent<MeshCollider>();
        }
        _lineRenderer.startWidth = 0.0f;
    }

    private void GenerateMeshCollider()
    {
        _lineRenderer.BakeMesh(_mesh, false);
        _collider.sharedMesh = _mesh;
    }

    public void BlastAttack()
    {
        _lineRenderer.startWidth = 1.0f;
        StartCoroutine(Blast());
    }

    private IEnumerator Blast()
    {
        float currentRadius = 0f;

        while(currentRadius < maxRadius)
        {
            if(currentRadius > 0.0f) GenerateMeshCollider();
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);
            yield return null;
        }
        _mesh.Clear();
        _collider.sharedMesh = null;
        Draw(0f);
        _lineRenderer.startWidth = 0.0f;
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360.0f / pointsCount;
        for(int i = 0;i <= pointsCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = dir * currentRadius;

            _lineRenderer.SetPosition(i, position);
        }
        _lineRenderer.widthMultiplier = Mathf.Lerp(0.0f, startWidth, 1f - currentRadius / maxRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit the player");
        }
    }
}
