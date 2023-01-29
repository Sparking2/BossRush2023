using UnityEngine;
using UnityEngine.Pool;

namespace Ammunition.Pool
{
    public class PoolManager : MonoBehaviour
    {
        // Collection checks will throw errors if we try to release an item that is already in the pool.
        public bool collectionChecks = true;
        public int maxPoolSize = 100;
        private IObjectPool<Projectile> _pool;

        [Header("Ammo Type"), SerializeField,]
        private Bullet prefabBullet;

        private Transform _playerTransform;
        
        public IObjectPool<Projectile> Pool
        {
            get
            {
                if ( _pool == null )
                {
                    _pool = new ObjectPool<Projectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                        OnDestroyPoolObject, collectionChecks, maxPoolSize);
                }

                return _pool;
            }
        }

        private void Start()
        {
            _playerTransform = FindObjectOfType<Player.ComponentPlayer>().transform;
        }

        private Projectile CreatePooledItem()
        {
            GameObject go = Instantiate(prefabBullet, Vector3.zero, Quaternion.identity, transform).gameObject;
            var bullet = go.GetComponent<Bullet>();

            // This is used to return ParticleSystems to the pool when they have stopped.
            var returnToPool = go.AddComponent<ReturnProjectileToPool>();
            returnToPool.Pool = Pool;

            return bullet;
        }

        // Called when an item is returned to the pool using Release
        private void OnReturnedToPool( Projectile projectile )
        {
            projectile.transform.SetPositionAndRotation(Vector3.zero,Quaternion.Euler(Vector3.zero));
            projectile.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        private void OnTakeFromPool( Projectile bullet )
        {
            // bullet.transform.position = _playerTransform.position;
            bullet.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        private void OnDestroyPoolObject( Projectile projectile )
        {
            Destroy(projectile.gameObject);
        }
    }
}