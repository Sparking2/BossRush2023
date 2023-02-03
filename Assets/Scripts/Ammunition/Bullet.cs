using System;
using Sandbox.Victor;
using UnityEngine;

namespace Ammunition
{
    public class Bullet : Projectile
    {
        private Rigidbody _rigidbody;
        private TrailRenderer _trailRenderer;

        [SerializeField]
        private float velocity = 1;

        private float _currentLife;
        [SerializeField]
        public float maxLife = 2.0f;

        private void Awake()
        {
            if ( !TryGetComponent(out _rigidbody) )
                throw new Exception("Can't find rigidbody");
            if ( !TryGetComponent(out _trailRenderer) )
                throw new Exception("Can't find trailRenderer");
        }

        private void Update()
        {
            _currentLife += Time.deltaTime;
            if ( _currentLife > maxLife )
            {
                Reset();
            }
        }

        private void OnTriggerEnter( Collider other ) => OnImpact(other);

        public override void Fire( Vector3 direction )
        {
            _trailRenderer.Clear();
            _trailRenderer.emitting = true;
            _trailRenderer.enabled = true;
            _rigidbody.velocity = direction * velocity;
        }

        public override void OnImpact(Collider impactedObject)
        {
            if ( impactedObject.TryGetComponent(out HealthComponent healthComponent) )
            {
                healthComponent.DoDamage(1.0f);
            }
            Reset();
        }

        public override void Reset()
        {
            _trailRenderer.Clear();
            _trailRenderer.emitting = false;
            _trailRenderer.enabled = false;
            _currentLife = 0.0f;
            _rigidbody.velocity = Vector3.zero;
            ResetEvent?.Invoke();
        }
    }
}