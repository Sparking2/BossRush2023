using System;
using UnityEngine.Pool;

namespace Ammunition.Pool
{
    [Serializable]
    public struct PoolData
    {
        public ProjectileType type;
        public Projectile prefab;
        public IObjectPool<Projectile> Pool;
    }
}