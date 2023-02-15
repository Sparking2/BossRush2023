using System;
using System.Collections;
using Ammunition;
using Ammunition.Pool;
using Enums;
using UnityEngine;

namespace Player
{
    public class ComponentWeapon : MonoBehaviour
    {
        public delegate void FireModeChange( FireMode mode );

        public delegate void FireModeRemainingChange( ushort remaining );

        public delegate void AmmoTypeChange( ProjectileType mode );

        public delegate void AmmoRemainingChange( ushort num );


        public FireModeChange OnFireModeChanged;
        public FireModeRemainingChange OnFireModeRemainingChange;
        public AmmoTypeChange OnAmmoTypeChanged;
        public AmmoRemainingChange OnAmmoRemainingChange;

        [SerializeField]
        private Transform weaponBarrelEnd;
        [SerializeField]
        private float fireCooldown = 1.0f;
        [SerializeField]
        private float burstShotSpawnCooldown = 0.1f;
        [SerializeField]
        private float waveShotCooldownMultiplier = 2.0f;
        [SerializeField]
        private FireMode currentFireMode = FireMode.Single;
        [SerializeField]
        private ProjectileType currentAmmoType = ProjectileType.Bullet;

        [SerializeField] private ParticleSystem shootParticles;

        private ComponentInput _input;
        private ComponentTarget _target;
        private bool _wasFiring;
        private float _currentCooldown;

        private int _remainingFireModeCharges;
        private int _remainingAmmoCharges;

        private void Start()
        {
            if ( !TryGetComponent(out _input) )
                throw new Exception($"Can't find {_input.GetType().Name} in player");

            _input.InputEventFire += HandleFire;

            if ( !TryGetComponent(out _target) )
                throw new Exception($"Can't find {_target.GetType().Name} in player");

            ChangeAmmoType(ProjectileType.Bullet);
            ChangeFireMode(FireMode.Single);
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

            if ( SoundManager.Instance )
                SoundManager.Instance.PlaySound(SoundType.Sfx, "0");

            _wasFiring = true;
            if(shootParticles) shootParticles.Play();
            _currentCooldown = 0;
            Projectile bullet = PoolManager.GetPool(currentAmmoType).Get();
            bullet.transform.SetPositionAndRotation(weaponBarrelEnd.position, Quaternion.identity);
            bullet.Fire(CalculateBulletDirection());
            ReduceCharges();
        }

        private void HandleBurstShot( in bool isFiring )
        {
            if ( !IsAllowedToShoot(isFiring) ) return;
            _wasFiring = true;
            if (shootParticles) shootParticles.Play();
            _currentCooldown = -fireCooldown;
            StartCoroutine(nameof( FireBurst ));
        }

        private void HandleWaveShot( in bool isFiring )
        {
            if ( !IsAllowedToShoot(isFiring) ) return;
            _wasFiring = true;
            if (shootParticles) shootParticles.Play();
            _currentCooldown = -fireCooldown * waveShotCooldownMultiplier;

            for ( var i = 0; i < 5; i++ )
            {
                Projectile bullet = PoolManager.GetPool(currentAmmoType).Get();
                var randomOffset = new Vector3(UnityEngine.Random.Range(-0.0625f, 0.0625f),
                    UnityEngine.Random.Range(-0.0625f, 0.0625f), UnityEngine.Random.Range(-0.0625f, 0.0625f));
                bullet.transform.SetPositionAndRotation(weaponBarrelEnd.position, Quaternion.identity);
                bullet.Fire(CalculateBulletDirection() + randomOffset);
                ReduceCharges();
            }
        }

        private void HandleAutoShot( in bool isFiring )
        {
            if ( !isFiring ) return;
            _currentCooldown = 0;
            if (shootParticles) shootParticles.Play();
            Projectile bullet = PoolManager.GetPool(currentAmmoType).Get();
            bullet.transform.SetPositionAndRotation(weaponBarrelEnd.position, transform.rotation);
            bullet.Fire(CalculateBulletDirection());
            ReduceCharges();
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

        public void ChangeFireMode( FireMode targetMode )
        {
            currentFireMode = targetMode;
            OnFireModeChanged?.Invoke(targetMode);
            _remainingFireModeCharges = targetMode != FireMode.Single ? 10 : 999;
            OnFireModeRemainingChange?.Invoke((ushort) _remainingFireModeCharges);
        }

        public void ChangeAmmoType( ProjectileType targetProjectile )
        {
            currentAmmoType = targetProjectile;
            OnAmmoTypeChanged?.Invoke(targetProjectile);
            _remainingAmmoCharges = targetProjectile != ProjectileType.Bullet ? 10 : 999;
            OnAmmoRemainingChange?.Invoke((ushort) _remainingAmmoCharges);
        }

        private Vector3 CalculateBulletDirection()
        {
            Vector3 heading = _target.CurrentTarget - weaponBarrelEnd.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            return direction;
        }

        private IEnumerator FireBurst()
        {
            for ( var i = 0; i < 3; i++ )
            {
                Projectile bullet = PoolManager.GetPool(currentAmmoType).Get();
                bullet.transform.SetPositionAndRotation(weaponBarrelEnd.position, transform.rotation);
                bullet.Fire(CalculateBulletDirection());
                yield return new WaitForSeconds(burstShotSpawnCooldown);
                ReduceCharges();
            }
        }

        private void ReduceCharges()
        {
            if ( currentFireMode != FireMode.Single )
            {
                _remainingFireModeCharges -= 1;
                OnFireModeRemainingChange?.Invoke((ushort) _remainingFireModeCharges);
                if ( _remainingFireModeCharges <= 0 )
                {
                    ChangeFireMode(FireMode.Single);
                }
            }

            if ( currentAmmoType != ProjectileType.Bullet )
            {
                _remainingAmmoCharges -= 1;
                OnAmmoRemainingChange?.Invoke((ushort) _remainingAmmoCharges);
                if ( _remainingAmmoCharges <= 0 )
                {
                    ChangeAmmoType(ProjectileType.Bullet);
                }
            }
        }
    }
}