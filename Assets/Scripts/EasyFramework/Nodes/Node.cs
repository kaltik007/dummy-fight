using System;
using EasyFramework.Context;
using EasyFramework.ReactiveTriggers;

namespace EasyFramework.Nodes
{
    public class Node<T> : IAttachableNode<T> where T : AbstractContext
    {
        public bool HasContext => _context is not null;
        public bool IsDisposed { get; private set; }
        public IReadOnlyReactiveTrigger Disposed => _disposedTrigger;
        
        private readonly ReactiveTrigger _disposedTrigger = new();
        private T _context;
        private IDisposable _disposableForParent;

        public void AttachTo(INode parent, T context)
        {
            _disposableForParent = parent.Disposed.SubscribeWithSkip(Dispose);
            _context = context;
            
            ApplyContext(context);
        }
        
        public void Dispose()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
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