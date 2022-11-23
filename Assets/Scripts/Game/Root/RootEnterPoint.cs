using DG.Tweening;
using EasyFramework.Context;
using EasyFramework.Nodes;
using Game.Root.Level;
using UnityEngine;

namespace Game.Root
{
    public class RootEnterPoint : NodeBehaviour<RootEnterPoint.NoContext>
    {
        [SerializeField] private RootConfig _config;
        [SerializeField] private UnityContext _unityContext;
        public record NoContext : AbstractContext;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            DOTween.SetTweensCapacity(500,50);
            
            var levelNode = new LevelNode();
            levelNode.AttachTo(this, new LevelNode.Context(
                _config,
                _unityContext));
        }
        
        private void OnDestroy()
        {
            Dispose();
        }
    }
}