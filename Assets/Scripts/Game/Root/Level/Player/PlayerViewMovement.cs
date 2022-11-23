using System;
using EasyFramework.Context;
using EasyFramework.Nodes;
using Joystick_Pack.Scripts.Joysticks;
using UniRx;
using UnityEngine;

namespace Game.Root.Level.Player
{
    public class PlayerViewMovement : NodeBehaviour<PlayerViewMovement.Context>
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private GameObject _characterModel;
        [SerializeField] private float _movementDefaultSpeed = 1.0f;
        
        public record Context(float MovementSpeedMultiplier,
            float RotationSpeedMultiplier,
            ReactiveProperty<bool> PlayerIsMoving,
            ReactiveProperty<Vector3> PlayerPosition, DynamicJoystick DynamicJoystick) : AbstractContext;

        private Context _ctx;
        private IDisposable _updateTimer;
        private DynamicJoystick _playerInput;
        
        protected override void ApplyContext(Context context)
        {
            _ctx = context;
            Init();
        }

        private void Init()
        {
            _playerInput = _ctx.DynamicJoystick;
            _updateTimer = Observable.IntervalFrame(1, FrameCountType.Update).Subscribe(UpdateMovement)
                .AddTo(this);
        }

        private void UpdateMovement(long nextTick)
        {
            UpdatePosition(Time.deltaTime);
            UpdateRotation(Time.deltaTime);
            
            if (Mathf.Abs(transform.position.y) > 0.001f)
            {
                var newPosition = transform.position;
                newPosition.y = 0f;
                transform.position = newPosition;
            }
        }

        private void UpdatePosition(float deltaTime)
        {
            _ctx.PlayerIsMoving.Value = _playerInput.Horizontal != 0 && _playerInput.Vertical != 0;

            var movement = Vector3.zero;
            movement += Vector3.forward * _playerInput.Vertical;
            movement += Vector3.right * _playerInput.Horizontal;
            movement *= _movementDefaultSpeed * _ctx.MovementSpeedMultiplier;
            movement *= deltaTime;

            _characterController.Move(movement);
            _ctx.PlayerPosition.Value = transform.position;
        }

        private void UpdateRotation(float deltaTime)
        {
            _playerInput = DynamicJoystick.Instance;
            if (!_ctx.PlayerIsMoving.Value) return;
            
            var angle = Vector2.Angle(_playerInput.Direction, Vector2.up);
            if (_playerInput.Horizontal < 0) angle = 360 - angle;

            var newRotation = Quaternion.Euler(0.0f, angle, 0.0f);
            _characterModel.transform.rotation = Quaternion.RotateTowards(_characterModel.transform.rotation, newRotation,
                _ctx.RotationSpeedMultiplier * deltaTime);
        }
    }
}