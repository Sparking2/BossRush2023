using System;
using Sandbox.Victor;
using UnityEngine;

namespace Ammunition
{
    public class ExplosiveArea : MonoBehaviour
    {
        private Mine _mineParent;

        private void Awake()
        {
            _mineParent = GetComponentInParent<Mine>();
        }

        private void OnTriggerEnter( Collider other )
        {
            if ( !other.TryGetComponent(out HealthComponent hp) ) return;
            
            hp.DoDamage(1.0f);
            // TODO: VFX?
            _mineParent.Reset();
        }
    }
}