//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventSystemGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class MarketingEventSystem : Entitas.ReactiveSystem<GameEntity> {

    readonly System.Collections.Generic.List<IMarketingListener> _listenerBuffer;

    public MarketingEventSystem(Contexts contexts) : base(contexts.game) {
        _listenerBuffer = new System.Collections.Generic.List<IMarketingListener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context) {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Added(GameMatcher.Marketing)
        );
    }

    protected override bool Filter(GameEntity entity) {
        return entity.hasMarketing && entity.hasMarketingListener;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities) {
        foreach (var e in entities) {
            var component = e.marketing;
            _listenerBuffer.Clear();
            _listenerBuffer.AddRange(e.marketingListener.value);
            foreach (var listener in _listenerBuffer) {
                listener.OnMarketing(e, component.clients, component.ClientList);
            }
        }
    }
}
