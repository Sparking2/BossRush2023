﻿using UnityEngine;

namespace Sandbox.Victor
{
    public abstract class Projectile : MonoBehaviour
    {
        public delegate void TriggerReset();
        public TriggerReset ResetEvent;

        public abstract void Fire();
        public abstract void OnImpact();

        public abstract void Reset();
    }
}