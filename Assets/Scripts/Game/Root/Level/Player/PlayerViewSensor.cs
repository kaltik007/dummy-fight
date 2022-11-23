using System;
using EasyFramework.Context;
using EasyFramework.Nodes;
using Game.Root.Level.Damageables;
using UniRx;
using UnityEngine;

namespace Game.Root.Level.Player
{
    public class PlayerViewSensor : NodeBehaviour<PlayerViewSensor.Context>
    {
        public record Context(
            PlayerConfig PlayerConfig,
            ReactiveProperty<IDamageable> MainDamageableTarget,
            ReactiveCollection<IDamageable> AllDamageableTargets,
            ReactiveProperty<bool> PlayerIsAttacking) : AbstractContext;
        
        private Context _ctx;
        
        private IDisposable _updateTimer;
        private IDamageable _mainDamageableTarget;
        private const string DamageableLayer = "Damageable";
        private int _damageableOverlapLayer;
        private readonly Collider[] _overlapResultDamageableColliders = new Collider[50];
        
        protected override void ApplyContext(Context context)
        {
            _ctx = context;
            Init();
        }

        private void Init()
        {
            _updateTimer = Observable.Interval(TimeSpan.FromMilliseconds(100)).Subscribe(UpdateDetections).AddTo(this);
            _damageableOverlapLayer = LayerMask.GetMask(DamageableLayer);
        }
        
        private void UpdateDetections(long nextTick)
        {
            UpdateDamageableDetections();
        }
        
        private void UpdateDamageableDetections()
        {
            _ctx.AllDamageableTargets.Clear();
            _mainDamageableTarget = null;
            
            var interactionPosition = transform.position;
            interactionPosition += transform.forward * _ctx.PlayerConfig.damageSphereOffsetForward;
            interactionPosition += transform.up * _ctx.PlayerConfig.damageSphereOffsetUp;
            var overlapResultsCount =
                Physics.OverlapSphereNonAlloc(interactionPosition, _ctx.PlayerConfig.damageSphereRadius, _overlapResultDamageableColliders, _damageableOverlapLayer);

            if (overlapResultsCount > 0)
            {
                for (var i = 0; i < overlapResultsCount; i++)
                {
                    var damageableView = _overlapResultDamageableColliders[i].gameObject.GetComponent<IDamageable>();
                    if (damageableView != null && !_ctx.AllDamageableTargets.Contains(damageableView) && !damageableView.IsDestroyed)
                    {
                        _ctx.AllDamageableTargets.Add(damageableView);
                    }
                }
            }
            
            UpdateMainDamageableTarget();
            _ctx.PlayerIsAttacking.Value = _ctx.MainDamageableTarget.Value != null;
        }
        
        private void UpdateMainDamageableTarget()
        {
            if (_ctx.AllDamageableTargets.Count == 0)
            {
                _mainDamageableTarget = null;
            }
            else if (_ctx.AllDamageableTargets.Count == 1)
            {
                _mainDamageableTarget = _ctx.AllDamageableTargets[0];
            }
            else 
            {
                var mainDamageableTargetDistance = float.MaxValue;

                foreach (var damageableTarget in _ctx.AllDamageableTargets)
                {
                    var damageableTargetDistance =
                        (damageableTarget.Position - transform.position).sqrMagnitude;

                    if (damageableTargetDistance > mainDamageableTargetDistance) continue;

                    mainDamageableTargetDistance = damageableTargetDistance;
                    _mainDamageableTarget = damageableTarget;
                }
            }

            _ctx.MainDamageableTarget.Value = _mainDamageableTarget;
        }
    }
}