using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    [SerializeField] private float maxFallingSpeed;
    [SerializeField] private float minFallingSpeed;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float warnDelay;
    [SerializeField] private ParticleSystem warningParticles;
    [SerializeField] private GameObject fallingObject;
    [SerializeField] private ParticleSystem impactParticles;

    private WaitForSeconds fallDelay;
    private void Awake()
    {
        warnDelay = Random.Range(warnDelay - 0.25f, warnDelay + 0.75f);
        fallDelay = new WaitForSeconds(warnDelay);
    }

    private void Start()
    {
        fallingSpeed = Random.Range(minFallingSpeed, maxFallingSpeed);
        StartCoroutine(FallingCorutine());

        Destroy(gameObject, 2.5f);
    }

    private IEnumerator FallingCorutine()
    {
        warningParticles.Play();
        warningParticles.transform.position = new Vector3(warningParticles.transform.position.x, warningParticles.transform.position.y + 1.1f, warningParticles.transform.position.z);
        yield return fallDelay;
        warningParticles.Stop();
        fallingObject.SetActive(true);
        while(Vector3.Distance(transform.position,fallingObject.transform.position) > 0.5f)
        {
            fallingObject.transform.position = Vector3.MoveTowards(fallingObject.transform.position, transform.position, fallingSpeed * Time.deltaTime);
            yield return null;
        }
        fallingObject.SetActive(false);
        impactParticles.Play();
    }
}
