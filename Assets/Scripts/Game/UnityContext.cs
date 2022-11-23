using Game.Root.Level.Damageables;
using Game.Root.Level.Damageables.Dummy;
using Game.Root.Level.Player;
using Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

namespace Game
{
    public class UnityContext : MonoBehaviour
    {
        [SerializeField] private GameObject _levelRoot;
        [SerializeField] private Camera _camera;
        [SerializeField] private DynamicJoystick _dynamicJoystick;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private DummyView[] _dummies;

        public GameObject LevelRoot => _levelRoot;
        public Camera Camera => _camera;
        public DynamicJoystick DynamicJoystick => _dynamicJoystick;
        public PlayerView PlayerView => _playerView;
        public DummyView[] DummyViews => _dummies;
    }
}