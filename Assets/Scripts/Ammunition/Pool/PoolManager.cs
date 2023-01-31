using System;
using System.Collections.Generic;
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

        [SerializeField]
        private PoolData[] projectileTypeArray;
        private readonly Dictionary<ProjectileType, PoolData> _projectileLibrary =
            new Dictionary<ProjectileType, PoolData>();

        private static PoolManager _instance;

        private void Start()
        {
            if ( _instance != null && _instance != this )
                Destroy(gameObject);
            else
                _instance = this;

            foreach ( PoolData poolData in projectileTypeArray )
            {
                if ( !_projectileLibrary.TryAdd(poolData.type, poolData) )
                    throw new Exception("Can't add item to library");
            }

            GetPool(ProjectileType.Bullet).Get();
            GetPool(ProjectileType.Boss).Get();
        }

        public static IObjectPool<Projectile> GetPool( ProjectileType type )
        {
            if ( !_instance._projectileLibrary.TryGetValue(type, out PoolData pool) )
                throw new Exception("Can't get pool");

            if ( pool.Pool == null )
            {
                pool.Pool = new ObjectPool<Projectile>(
                    () =>
                    {
                        Projectile prefab = pool.prefab;
                        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, _instance.transform)
                            .gameObject;
                        var bullet = go.GetComponent<Projectile>();
                        // This is used to return Projectile to the pool when they have stopped.
                        var returnToPool = go.AddComponent<ReturnProjectileToPool>();
                        returnToPool.Pool = pool.Pool;
                        return bullet;
                    },
                    _instance.OnTakeFromPool,
                    _instance.OnReturnedToPool,
                    _instance.OnDestroyPoolObject,
                    _instance.collectionChecks,
                    _instance.maxPoolSize
                );
                _instance._projectileLibrary[type] = pool;
            }

            return pool.Pool;
        }
        
        // Called when an item is returned to the pool using Release
        private void OnReturnedToPool( Projectile projectile )
        {
            projectile.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
            projectile.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        private void OnTakeFromPool( Projectile bullet )
        {
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