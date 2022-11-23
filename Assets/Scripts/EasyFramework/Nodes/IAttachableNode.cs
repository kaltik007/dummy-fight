using EasyFramework.Context;

namespace EasyFramework.Nodes
{
    public interface IAttachableNode<T> : INode where T : AbstractContext
    {
        void AttachTo(INode parent, T context);
    }
}