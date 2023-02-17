using UnityEngine;

namespace Ammunition
{
    public abstract class Projectile : MonoBehaviour
    {
        public delegate void TriggerReset();
        public TriggerReset ResetEvent;

        public abstract void Fire( Vector3 direction );

        public abstract void OnImpact(GameObject impactedGameObject);

        public abstract void Reset();
    }
}