using EasyFramework.Context;
using EasyFramework.Nodes;
using EasyFramework.ReactiveTriggers;

namespace Game.Root.Level.Player
{
    public class PlayerAnimationEventListener : NodeBehaviour<PlayerAnimationEventListener.Context>
    {
        public record Context(
            ReactiveTrigger OnAttackHit) : AbstractContext;
        
        private Context _ctx;

        protected override void ApplyContext(Context context)
        {
            _ctx = context;
        }

        public void Notify()
        {
            _ctx.OnAttackHit.Notify();
        }
    }
}