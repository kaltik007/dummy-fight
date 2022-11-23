using System;
using EasyFramework.Context;
using EasyFramework.ReactiveTriggers;

namespace EasyFramework.Nodes
{
    public interface INode : IContextful, IDisposable
    {
        IReadOnlyReactiveTrigger Disposed { get; }
    }
}