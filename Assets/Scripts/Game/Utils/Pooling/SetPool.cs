using System.Collections.Generic;
using EasyFramework.Context;
using EasyFramework.Nodes;
using UnityEngine;

namespace Game.Utils.Pooling
{
    public class SetPool : Node<SetPool.Context>
    {
        public record Context() : AbstractContext;
        private readonly GameObject _spawnGameObject;
        private readonly Stack<GameObject> _despawnPool = new Stack<GameObject>();
        private readonly HashSet<GameObject> _setActiveObjects = new HashSet<GameObject>();

        public IReadOnlyCollection<GameObject> SetActiveObjects => _setActiveObjects;
        
        public SetPool(GameObject spawnGameObject)
        {
            _spawnGameObject = spawnGameObject;
        }

        public GameObject Spawn(Transform transform, bool worldPositionStays = false)
        {
            GameObject instance;
            if (_despawnPool.Count > 0)
            {
                instance = _despawnPool.Pop();
                instance.transform.SetParent(transform);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;
                instance.transform.localScale = Vector3.one;
            }
            else
            {
                instance = Object.Instantiate(_spawnGameObject, transform);
            }
            instance.SetActive(true);
            _setActiveObjects.Add(instance);
            return instance;
        }

        public T Spawn<T>(Transform transform, bool worldPositionStays = false)
        {
            GameObject go = Spawn(transform, worldPositionStays);
            var component = go.GetComponent<T>();
            
            if(component == null)
                DeSpawn(go);
            return component;
        }

        public void DeSpawn(GameObject gameObject)
        {
            if (_setActiveObjects.Contains(gameObject))
            {
                _despawnPool.Push(gameObject);
                _setActiveObjects.Remove(gameObject);
                gameObject.SetActive(false);
            }
        }

        public void DeSpawnAll()
        {
            foreach (var gameObject in _setActiveObjects)
            {
                gameObject.SetActive(false);
                _despawnPool.Push(gameObject);
            }

            _setActiveObjects.Clear();
        }
    }
}