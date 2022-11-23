using System;
using EasyFramework.Context;
using EasyFramework.Nodes;
using EasyFramework.ReactiveEvents;
using EasyFramework.ReactiveTriggers;
using Game.Root.Level.Damageables;
using Game.Utils.Saving;
using Joystick_Pack.Scripts.Joysticks;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace Game.Root.Level.Player
{
    public class PlayerNode : Node<PlayerNode.Context>
    {
        public record Context(
            PlayerView PlayerView,
            PlayerConfig PlayerConfig,
            ReactiveProperty<Vector3> PlayerPosition,
            DynamicJoystick DynamicJoystick,
            ReactiveEvent<uint> OnRecieveExperience) : AbstractContext;

        private Context _ctx;
        private const string Key = "player_state";
        private ReactiveProperty<PlayerData> _playerState;
        private ReactiveProperty<IDamageable> _mainDamageableTarget;
        private ReactiveCollection<IDamageable> _allDamageableTargets;
        private ReactiveTrigger _onAttackHit;
        private ReactiveEvent<uint> _onPlayerLevelUp;
        protected override void ApplyContext(Context context)
        {
            _ctx = context;
            Init();
        }

        private void Init()
        {
            _playerState = new ReactiveProperty<PlayerData>();
            LoadPlayerState();
            
            _mainDamageableTarget = new ReactiveProperty<IDamageable>();
            _allDamageableTargets = new ReactiveCollection<IDamageable>();
            _onAttackHit = new ReactiveTrigger();
            _onPlayerLevelUp = new ReactiveEvent<uint>();
            
            InitPlayerView();

            _onAttackHit.SubscribeWithSkip(PerformAttack);
            _ctx.OnRecieveExperience.SubscribeWithSkip(PlayerRecievedExperience);
        }

        private void InitPlayerView()
        {
            _ctx.PlayerView.AttachTo(this, new PlayerView.Context(
                _ctx.PlayerConfig,
                _ctx.PlayerPosition,
                _ctx.DynamicJoystick,
                _onPlayerLevelUp,
                _onAttackHit,
                _mainDamageableTarget,
                _allDamageableTargets));
        }
        
        private void PerformAttack()
        {
            foreach (var damageable in _allDamageableTargets)
            {
                damageable.TakeDamage(1);
            }
        }

        private void PlayerRecievedExperience(uint amount)
        {
            
        }
        
        [Serializable]
        private record PlayerSaveParams(ReactiveProperty<PlayerData> PlayerState) : SaveParams
        {
            public PlayerSaveParams() : this(new ReactiveProperty<PlayerData>())
            {
            }
            public override string ToJson() => JsonConvert.SerializeObject(this);
            public override SaveParams FromJson(string json) => JsonConvert.DeserializeObject<PlayerSaveParams>(json);
        }
        
        [Serializable]
        private class PlayerData
        {
            public ReactiveProperty<uint> playerLevel;
            public ReactiveProperty<uint> playerExperience;

            public PlayerData(ReactiveProperty<uint> level, ReactiveProperty<uint> exp)
            {
                playerLevel = level;
                playerExperience = exp;
            }
        }

        private void LoadPlayerState()
        {
            if (PlayerPrefs.HasKey(Key))
            {
                var loadedState = JsonConvert.DeserializeObject<ReactiveProperty<PlayerData>>(
                    PlayerPrefs.GetString(Key));
                if (loadedState != null)
                    _playerState.Value = loadedState.Value;
            }
            else
            {
                _playerState.Value = new PlayerData(
                    new ReactiveProperty<uint>(),
                    new ReactiveProperty<uint>());
            }
        }

        private void SavePlayerState()
        {
            var json = JsonConvert.SerializeObject(_playerState);
            PlayerPrefs.SetString(Key, json);
            PlayerPrefs.Save();
        }
    }
}