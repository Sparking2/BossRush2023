using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWaveControl : MonoBehaviour
{
    private BlastWave m_blastWave;
    [SerializeField] private ParticleSystem explotionParticles;
    private float lifeTime = 3.0f;
    private void Awake()
    {
        m_blastWave = GetComponentInChildren<BlastWave>();
    }

    private void Start()
    {
        m_blastWave.BlastAttack();
        explotionParticles.Play();
        Destroy(gameObject, lifeTime);
    }
}
