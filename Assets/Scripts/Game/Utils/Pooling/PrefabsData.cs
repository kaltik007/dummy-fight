using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils.Pooling
{
    [CreateAssetMenu(fileName = "Spawn Prefabs Data", menuName = "GameData/Spawn Prefabs Data")]
    public class PrefabsData : ScriptableObject
    {
        [SerializeField] private List<PrefabIdPool.SpawnInfo> _listSpawnPrefabs;

        public IReadOnlyList<PrefabIdPool.SpawnInfo> ListPrefabs => _listSpawnPrefabs;
    }
}