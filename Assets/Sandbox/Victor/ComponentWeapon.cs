using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sandbox.Victor
{
    public class ComponentWeapon : MonoBehaviour
    {
        private ComponentInput _input;

        [SerializeField]
        private Transform weaponBarrelEnd;

        private Transform _mainCamera;

        [SerializeField]
        private float fireCooldown = 1.0f;
        [SerializeField]
        private float burstShotSpawnCooldown = 0.1f;
        [SerializeField]
        private float waveShotCooldownMultiplier = 2.0f;
        
        private float _currentCooldown = 0.0f;

        private PoolManager _poolManager;

        [SerializeField]
        private FireMode currentFireMode = FireMode.Single;
        private bool _wasFiring;

        private void Start()
        {
            if ( !TryGetComponent(out _input) )
                throw new Exception($"Can't find {_input.GetType().Name} in player");

            _input.InputEventFire += HandleFire;

            if ( Camera.main != null )
                _mainCamera = Camera.main.transform;
            else
                throw new Exception($"There is no main camera!!");

            _poolManager = FindObjectOfType<PoolManager>();
            if ( !_poolManager )
                throw new Exception($"Can't find Pool!!!");
        }

        private void Update()
        {
            _currentCooldown += Time.deltaTime;
        }

        private void OnDestroy()
        {
            if ( _input != null )
            {
                _input.InputEventFire -= HandleFire;
            }
        }

        private void HandleFire( float isFired )
        {
            bool isFiring = !isFired.Equals(0);
            if ( _currentCooldown < fireCooldown ) return;
            switch ( currentFireMode )
            {
                case FireMode.Single:
                    HandleSingleShot(isFiring);
                    break;
                case FireMode.Burst:
                    HandleBurstShot(isFiring);
                    break;
                case FireMode.Wave:
                    HandleWaveShot(isFiring);
                    break;
                case FireMode.Auto:
                    HandleAutoShot(isFiring);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleSingleShot( in bool isFiring )
        {
            if ( !IsAllowedToShoot(isFiring) ) return;

            _wasFiring = true;
            _currentCooldown = 0;
            Projectile bullet = _poolManager.Pool.Get();
            bullet.transform.SetPositionAndRotation(weaponBarrelEnd.position, transform.rotation);
            bullet.Fire();
        }

        private void HandleBurstShot( in bool isFiring )
        {
            if ( !IsAllowedToShoot(isFiring) ) return;
            _wasFiring = true;
            _currentCooldown = -fireCooldown;
            StartCoroutine(nameof( FireBurst ));
        }

        private void HandleWaveShot( in bool isFiring )
        {
            if ( !IsAllowedToShoot(isFiring) ) return;
            _wasFiring = true;
            _currentCooldown = -fireCooldown * waveShotCooldownMultiplier;
            
            for ( var i = 0; i < 15; i++ )
            {
                Projectile bullet = _poolManager.Pool.Get();
                var randomOffset = new Vector3(UnityEngine.Random.Range(-10.0f,10.0f),UnityEngine.Random.Range(-10.0f,10.0f),0);
                Vector3 rotation = transform.rotation.eulerAngles + randomOffset;
                bullet.transform.SetPositionAndRotation(weaponBarrelEnd.position, Quaternion.Euler(rotation));
                bullet.Fire();
            }
        }

        private void HandleAutoShot( in bool isFiring )
        {
            if ( !isFiring ) return;
            _currentCooldown = 0;
            Projectile bullet = _poolManager.Pool.Get();
            bullet.transform.SetPositionAndRotation(weaponBarrelEnd.position, transform.rotation);
            bullet.Fire();
        }

        private bool IsAllowedToShoot( in bool isFiring )
        {
            if ( !_wasFiring && !isFiring || _wasFiring && isFiring )
            {
                return false;
            }
            else if ( _wasFiring && !isFiring )
            {
                _wasFiring = false;
                return false;
            }

            return true;
        }

        private IEnumerator FireBurst()
        {
            for ( var i = 0; i < 3; i++ )
            {
                Projectile bullet = _poolManager.Pool.Get();
                bullet.transform.SetPositionAndRotation(weaponBarrelEnd.position, transform.rotation);
                bullet.Fire();
                yield return new WaitForSeconds(burstShotSpawnCooldown);
            }
        }
    }
}