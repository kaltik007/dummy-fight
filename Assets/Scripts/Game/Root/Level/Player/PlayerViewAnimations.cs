using EasyFramework.Context;
using EasyFramework.Nodes;
using UniRx;
using UnityEngine;

namespace Game.Root.Level.Player
{
    public class PlayerViewAnimations : NodeBehaviour<PlayerViewAnimations.Context>
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _keyRun = "Run";
        [SerializeField] private string _keyAttack = "Attack";
        [SerializeField] private string _keyAttackSpeedMultiplier = "AttackSpeed";
        [SerializeField] private string _keyMovementSpeedMultiplier = "RunSpeed";
        public record Context(
            float AttackSpeedMultiplier,
            float MovementSpeedMultiplier,
            IReadOnlyReactiveProperty<bool> PlayerIsMoving,
            IReadOnlyReactiveProperty<bool> PlayerIsAttacking) : AbstractContext;

        private Context _ctx;
        protected override void ApplyContext(Context context)
        {
            _ctx = context;
            Init();
        }

        private void Init()
        {
            _animator.SetFloat(_keyAttackSpeedMultiplier, _ctx.AttackSpeedMultiplier);
            _animator.SetFloat(_keyMovementSpeedMultiplier, _ctx.MovementSpeedMultiplier);
            
            _ctx.PlayerIsMoving.Subscribe(OnPlayerIsMovingChange).AddTo(this);
            _ctx.PlayerIsAttacking.Subscribe(OnPlayerIsAttackingChange).AddTo(this);
        }

        private void OnPlayerIsMovingChange(bool isMoving)
        {
            _animator.SetBool(_keyRun, isMoving);
        }

        private void OnPlayerIsAttackingChange(bool isAttacking)
        {
            _animator.SetBool(_keyAttack, isAttacking);
        }
    }
}