using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Ammunition.Pool
{
    public class ReturnProjectileToPool : MonoBehaviour
    {
        public Projectile projectile;
        public IObjectPool<Projectile> Pool;

        private void OnEnable()
        {
            if ( !projectile )
                projectile = GetComponent<Projectile>();

            projectile.ResetEvent += OnItemReset;
        }

        private void OnDisable()
        {
            projectile.ResetEvent -= OnItemReset;
        }

        private void OnItemReset()
        {
            Pool.Release(projectile);
        }
    }
}