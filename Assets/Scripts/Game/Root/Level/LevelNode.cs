using EasyFramework.Context;
using EasyFramework.Nodes;
using EasyFramework.ReactiveEvents;
using Game.Root.Level.Damageables.Dummy;
using Game.Root.Level.Player;
using UniRx;
using UnityEngine;

namespace Game.Root.Level
{
    public class LevelNode : Node<LevelNode.Context>
    {
        public record Context(
            RootConfig Config,
            UnityContext UnityContext) : AbstractContext;

        private Context _ctx;

        private ReactiveProperty<Vector3> _playerPosition;
        private ReactiveEvent<uint> _onPlayerRecieveExperience;
        protected override void ApplyContext(Context context)
        {
            _ctx = context;
            Init();
        }

        private void Init()
        {
            _playerPosition = new ReactiveProperty<Vector3>();
            _onPlayerRecieveExperience = new ReactiveEvent<uint>();
            
            InitPlayer();
            InitDummies();
        }

        private void InitPlayer()
        {
            if (_ctx.UnityContext.PlayerView != null)
            {
                var playerNode = new PlayerNode();
                playerNode.AttachTo(this, new PlayerNode.Context(
                    _ctx.UnityContext.PlayerView,
                    _ctx.Config.PlayerConfig,
                    _playerPosition,
                    _ctx.UnityContext.DynamicJoystick,
                    _onPlayerRecieveExperience));
            }
        }

        private void InitDummies()
        {
            var dummyNode = new DummyNode();
            dummyNode.AttachTo(this, new DummyNode.Context(
                _ctx.UnityContext.DummyViews));
        }
    }
}