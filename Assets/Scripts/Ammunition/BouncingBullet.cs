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
            Debug.Log(impactedObject.gameObject.name);
        }

        public override void Reset()
        {
            _trailRenderer.Clear();
            // _trailRenderer.emitting = false;
            // _trailRenderer.enabled = false;
            _currentLife = 0.0f;
            _rigidbody.velocity = Vector3.zero;
            ResetEvent?.Invoke();
        }
    }
}