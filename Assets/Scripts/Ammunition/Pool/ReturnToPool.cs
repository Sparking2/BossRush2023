using UnityEngine;
using UnityEngine.Pool;

namespace Ammunition.Pool
{
    public class ReturnProjectileToPool : MonoBehaviour
    {
        public Projectile projectile;
        public IObjectPool<Projectile> Pool;

        private void Start()
        {
            projectile = GetComponent<Projectile>();
            projectile.ResetEvent += OnItemReset;
        }

        private void OnItemReset()
        {
            Pool.Release(projectile);
        }
    }
}