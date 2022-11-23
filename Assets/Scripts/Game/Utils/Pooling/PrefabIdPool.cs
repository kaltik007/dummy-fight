using System;
using System.Collections.Generic;
using EasyFramework.Context;
using EasyFramework.Nodes;
using UnityEngine;

namespace Game.Utils.Pooling
{
    public class PrefabIdPool : Node<PrefabIdPool.Context>
    {
        [Serializable]
        public struct SpawnInfo
        {
            public string id;
            public GameObject prefab;
        }

        public record Context() : AbstractContext;
        
        private Dictionary<string, SetPool> _dictionaryPools;

        public PrefabIdPool(IReadOnlyList<SpawnInfo> _datas)
        {
            _dictionaryPools = new Dictionary<string, SetPool>(_datas.Count);
            foreach (var info in _datas)
            {
                _dictionaryPools[info.id] = new SetPool(info.prefab);
            }
        }
        
        public T Spawn<T>(string key, Transform transform, bool worldPositionStays = false)
        {
            return _dictionaryPools.ContainsKey(key) ? _dictionaryPools[key].Spawn<T>(transform, worldPositionStays) : default;
        }


        public GameObject Spawn(string key, Transform transform, bool worldPositionStays = false)
        {
            return _dictionaryPools.ContainsKey(key) ? _dictionaryPools[key].Spawn(transform, worldPositionStays) : null;
        }

        public void DeSpawn(string key, GameObject component)
        {
            if (_dictionaryPools.ContainsKey(key))
            {
                _dictionaryPools[key].DeSpawn(component);
            }
        }

        public void DeSpawnAll()
        {
            foreach (var kvp in _dictionaryPools)
            {
                kvp.Value.DeSpawnAll();
            }
        }
    }
}