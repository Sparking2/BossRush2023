using System;
using Sandbox.Victor;
using UnityEngine;

namespace Ammunition
{
    public class BouncingBullet : Projectile
    {
        private Rigidbody _rigidbody;
        private TrailRenderer _trailRenderer;
        private float _currentLife;
        private ushort _currentBounce = 0;

        [SerializeField]
        private float velocity = 150;
        [SerializeField]
        public float maxLife = 2.0f;
        [SerializeField]
        private ushort maxBonceCount = 10;
        [SerializeField] private GameObject hitPrefab;

        private void Awake()
        {
            if ( !TryGetComponent(out _rigidbody) )
                throw new Exception("Can't find rigidbody");
            if ( !TryGetComponent(out _trailRenderer) )
                throw new Exception("Can't find trailRenderer");

            _rigidbody.useGravity = false;
        }

        private void Update()
        {
            _currentLife += Time.deltaTime;
            if ( _currentLife > maxLife )
            {
                Reset();
            }
        }

        public override void Fire( Vector3 direction )
        {
            _trailRenderer.Clear();
            _trailRenderer.emitting = true;
            _trailRenderer.enabled = true;
            _rigidbody.velocity = direction * velocity;
        }

        private void OnCollisionEnter( Collision collision ) => OnImpact(collision);

        public override void OnImpact( Collision impactedObject )
        {
            _currentLife = 0.0f;
            _currentBounce++;

            if (impactedObject.gameObject.TryGetComponent(out ComponentHealth hp) )
            {
                Debug.Log("hit something" + hp.name);
                hp.ReduceHealth(WeaponInfo.bouncyBulletDamage);
            }

            if (hitPrefab) Instantiate(hitPrefab, transform.position, Quaternion.identity);

            if ( _currentBounce > maxBonceCount )
                Reset();
        }

        public override void Reset()
        {
            _trailRenderer.Clear();
            // _trailRenderer.emitting = false;
            // _trailRenderer.enabled = false;
            _currentLife = 0.0f;
            _currentBounce = 0;
            _rigidbody.velocity = Vector3.zero;
            ResetEvent?.Invoke();
        }
    }
}