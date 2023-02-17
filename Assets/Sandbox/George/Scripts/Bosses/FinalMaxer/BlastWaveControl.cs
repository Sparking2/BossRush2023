using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWaveControl : MonoBehaviour
{
    private BlastWave m_blastWave;
    [SerializeField] private ParticleSystem explotionParticles;
    private float lifeTime = 3.0f;
    private Collider collider;
    private void Awake()
    {
        collider = GetComponent<Collider>();
        m_blastWave = GetComponentInChildren<BlastWave>();
    }

    private void Start()
    {
        m_blastWave.BlastAttack();
        explotionParticles.Play();
        Destroy(gameObject, lifeTime);
        Invoke("DeactivateCollision", 0.5f);
    }

    private void DeactivateCollision()
    {
        collider.enabled = false;
    }
}
