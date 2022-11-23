using System;
using EasyFramework.Context;
using EasyFramework.ReactiveTriggers;
using UniRx;
using UnityEngine;

namespace EasyFramework.Nodes
{
    public abstract class NodeBehaviour<T> : MonoBehaviour, IAttachableNode<T> where T : AbstractContext
    {
        public bool HasContext => _context is not null;
        public IReadOnlyReactiveTrigger Disposed => _disposedTrigger;
        public bool IsDisposed { get; private set; }
        
        private readonly ReactiveTrigger _disposedTrigger = new();
        private T _context;
        private IDisposable _disposableForParent;
        
        public void AttachTo(INode parent, T context)
        {
            _disposableForParent = parent.Disposed.SubscribeWithSkip(Dispose).AddTo(this);
            _context = context;
            
            ApplyContext(context);
        }
        
        public void Dispose()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(name);
            }

            IsDisposed = true;
            _disposedTrigger.Notify();
            _disposableForParent?.Dispose();
            
            OnDispose();
        }
        
        protected virtual void ApplyContext(T context)
        {
        }

        protected virtual void OnDispose()
        {
        }
    }
}