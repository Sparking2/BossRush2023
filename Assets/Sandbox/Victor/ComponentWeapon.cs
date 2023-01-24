using System;
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
        private float fireCooldown =  1.0f;

        private float _currentCooldown = 0.0f;

        private PoolManager _poolManager;
        
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
            if(!_poolManager)
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
            Debug.DrawLine(weaponBarrelEnd.position,_mainCamera.forward * float.MaxValue,Color.green);
            if(isFired.Equals(0f)) return;
            if ( _currentCooldown < fireCooldown ) return;
            _currentCooldown = 0;
            Projectile bullet = _poolManager.Pool.Get();
            bullet.transform.SetPositionAndRotation(weaponBarrelEnd.position,transform.rotation);
            bullet.Fire();
        }
    }
}