using System;
using UnityEngine;

namespace Ammunition
{
    public class Mine : Projectile
    {
        private enum MineState
        {
            Launched,
            Armed,
            Exploding,
        }

        private Rigidbody _rigidbody;
        private TrailRenderer _trailRenderer;

        [SerializeField]
        private float velocity = 75;
        [SerializeField]
        public float maxLife = 10.0f;
        [SerializeField]
        private Transform mineDamageArea;

        private float _currentLife;
        private MineState _currentMineState = MineState.Launched;

        private void Awake()
        {
            if ( !TryGetComponent(out _rigidbody) )
                throw new Exception("Can't find rigidbody");
            if ( !TryGetComponent(out _trailRenderer) )
                throw new Exception("Can't find trailRenderer");

            _rigidbody.useGravity = true;
        }

        private void Update()
        {
            if ( _currentMineState is MineState.Armed or MineState.Exploding )
                return;

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
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _currentMineState = MineState.Armed;
            if ( mineDamageArea != null )
            {
                GetComponent<Collider>().enabled = false;
                mineDamageArea.gameObject.SetActive(true);
            }
        }

        public override void Reset()
        {
            _trailRenderer.Clear();
            _trailRenderer.emitting = false;
            _trailRenderer.enabled = false;
            _currentMineState = MineState.Launched;
            mineDamageArea.gameObject.SetActive(false);
            GetComponent<Collider>().enabled = true;
            _currentLife = 0.0f;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            ResetEvent?.Invoke();
        }
    }
}