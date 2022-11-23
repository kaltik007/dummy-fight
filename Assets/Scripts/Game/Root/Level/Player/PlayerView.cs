using EasyFramework.Context;
using EasyFramework.Nodes;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using Game.Root.Level.Damageables;
using Joystick_Pack.Scripts.Joysticks;
using UniRx;
using UnityEngine;

namespace Game.Root.Level.Player
{
    public class PlayerView : NodeBehaviour<PlayerView.Context>
    {
        [SerializeField] private PlayerViewMovement _movement;
        [SerializeField] private PlayerViewSensor _sensor;
        [SerializeField] private PlayerViewAnimations _animations;
        [SerializeField] private PlayerAnimationEventListener _animationEventListener;
        public record Context(
            PlayerConfig PlayerConfig,
            ReactiveProperty<Vector3> PlayerPosition,
            DynamicJoystick DynamicJoystick,
            ReactiveEvent<uint> OnPlayerLevelUp,
            ReactiveTrigger OnPlayerAttackHit,
            ReactiveProperty<IDamageable> MainDamageableTarget,
            ReactiveCollection<IDamageable> AllDamageableTargets) : AbstractContext;

        private Context _ctx;
        
        private ReactiveProperty<bool> _playerIsAttacking;
        private ReactiveProperty<bool> _playerIsMoving;

        protected override void ApplyContext(Context context)
        {
            _ctx = context;
            Init();
        }

        private void Init()
        {
            _playerIsAttacking = new ReactiveProperty<bool>();
            _playerIsMoving = new ReactiveProperty<bool>();
            
            InitSensor();
            InitMovement();
            InitAnimations();
        }

        private void InitSensor()
        {
            _sensor?.AttachTo(this, new PlayerViewSensor.Context(
                _ctx.PlayerConfig,
                _ctx.MainDamageableTarget,
                _ctx.AllDamageableTargets,
                _playerIsAttacking));
        }

        private void InitMovement()
        {
            _movement?.AttachTo(this, new PlayerViewMovement.Context(
                _ctx.PlayerConfig.movementSpeedMultiplier,
                _ctx.PlayerConfig.rotationSpeedMultiplier,
                _playerIsMoving,
                _ctx.PlayerPosition,
                _ctx.DynamicJoystick));
        }

        private void InitAnimations()
        {
            _animations?.AttachTo(this, new PlayerViewAnimations.Context(
                _ctx.PlayerConfig.attackSpeedMultiplier,
                _ctx.PlayerConfig.movementSpeedMultiplier,
                _playerIsMoving,
                _playerIsAttacking));
            
            _animationEventListener?.AttachTo(this, new PlayerAnimationEventListener.Context(
                _ctx.OnPlayerAttackHit));
        }
    }
}