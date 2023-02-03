using System;
using UnityEngine;

namespace Ammunition
{
    public class BouncingBullet : Projectile
    {
        private Rigidbody _rigidbody;
        private TrailRenderer _trailRenderer;

        [SerializeField]
        private float velocity = 150;
        [SerializeField]
        public float maxLife = 2.0f;

        private float _currentLife;

        public override void Fire( Vector3 direction )
        {
            _trailRenderer.Clear();
            _trailRenderer.emitting = true;
            _trailRenderer.enabled = true;
            _rigidbody.velocity = direction * velocity;
        }

        private void OnTriggerEnter( Collider other ) => OnImpact(other);

        public override void OnImpact( Collider impactedObject ) { }

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