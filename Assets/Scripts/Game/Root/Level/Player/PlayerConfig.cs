using System;

namespace Game.Root.Level.Player
{
    [Serializable]
    public struct PlayerConfig
    {
        public float damageSphereRadius;
        public float damageSphereOffsetForward;
        public float damageSphereOffsetUp;
        public float[] damageLevels;
        
        public float movementSpeedMultiplier;
        public float rotationSpeedMultiplier;
        public float attackSpeedMultiplier;

        public uint[] experienceLevels;
    }
}