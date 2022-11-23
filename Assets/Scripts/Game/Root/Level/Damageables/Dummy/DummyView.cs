using EasyFramework.Context;
using EasyFramework.Nodes;
using EasyFramework.ReactiveTriggers;
using UnityEngine;

namespace Game.Root.Level.Damageables.Dummy
{
    public class DummyView : NodeBehaviour<DummyView.Context>, IDamageable
    {
        [SerializeField] private DummyAnimations _animations;

        public record Context() : AbstractContext;

        public Vector3 Position { get; }
        public bool IsDestroyed => false;

        private Context _ctx;
        private ReactiveTrigger _onHit;

        protected override void ApplyContext(Context context)
        {
            _ctx = context;
            Init();
        }

        private void Init()
        {
            _onHit = new ReactiveTrigger();
            _animations?.AttachTo(this, new DummyAnimations.Context(
                _onHit));
        }

        public void TakeDamage(float amount)
        {
            _onHit.Notify();
        }
    }
}