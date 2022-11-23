using UnityEngine;

namespace Game.Root.Level.Damageables
{
    public interface IDamageable
    {
        public bool IsDestroyed { get; }
        public void TakeDamage(float amount);
        public Vector3 Position { get; }
    }
}