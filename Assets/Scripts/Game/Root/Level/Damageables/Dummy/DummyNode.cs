using EasyFramework.Context;
using EasyFramework.Nodes;

namespace Game.Root.Level.Damageables.Dummy
{
    public class DummyNode : Node<DummyNode.Context>
    {
        public record Context(
            DummyView[] DummyViews) : AbstractContext;

        private Context _ctx;

        protected override void ApplyContext(Context context)
        {
            _ctx = context;
            Init();
        }

        private void Init()
        {
            foreach (var dummyView in _ctx.DummyViews)
            {
                dummyView.AttachTo(this, new DummyView.Context());
            }
        }
    }
}