using UnityEngine;

namespace Sandbox.Victor
{
    public class ComponentHealth
    {
        public ComponentHealth( float initialLife )
        {
            CurrentLife = initialLife;
        }

        public float CurrentLife;
        private void TakeDamage() { }
        private void RestoreHealth() { }
    }
}