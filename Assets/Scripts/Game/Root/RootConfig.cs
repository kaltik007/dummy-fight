using System;
using Game.Root.Level.Player;
using UnityEngine;

namespace Game.Root
{
    [CreateAssetMenu(fileName = "Root Game Config", menuName = "GameData/Root Config", order = 0)]
    public class RootConfig : ScriptableObject
    {
        [SerializeField] private PlayerConfig _playerConfig;

        public PlayerConfig PlayerConfig => _playerConfig;
    }
}