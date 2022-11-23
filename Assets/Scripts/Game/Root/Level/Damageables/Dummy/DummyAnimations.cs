using EasyFramework.Context;
using EasyFramework.Nodes;
using EasyFramework.ReactiveTriggers;
using UniRx;
using UnityEngine;

namespace Game.Root.Level.Damageables.Dummy
{
    public class DummyAnimations : NodeBehaviour<DummyAnimations.Context>
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _hitTriggerKey = "Hit";
        [SerializeField] private ParticleSystem _hitFx;
        public record Context(
            IReadOnlyReactiveTrigger OnHit) : AbstractContext;

        private Context _ctx;

        protected override void ApplyContext(Context context)
        {
            _ctx = context;
            Init();
        }

        private void Init()
        {
            _ctx.OnHit.SubscribeWithSkip(OnDummyHit).AddTo(this);
        }

        private void OnDummyHit()
        {
            //_animator?.SetTrigger(_hitTriggerKey);
            
            _hitFx?.Play();
        }
    }
}