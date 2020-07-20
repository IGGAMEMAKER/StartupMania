//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public MarketingComponent marketing { get { return (MarketingComponent)GetComponent(GameComponentsLookup.Marketing); } }
    public bool hasMarketing { get { return HasComponent(GameComponentsLookup.Marketing); } }

    public void AddMarketing(long newClients, System.Collections.Generic.Dictionary<int, long> newClientList) {
        var index = GameComponentsLookup.Marketing;
        var component = (MarketingComponent)CreateComponent(index, typeof(MarketingComponent));
        component.clients = newClients;
        component.ClientList = newClientList;
        AddComponent(index, component);
    }

    public void ReplaceMarketing(long newClients, System.Collections.Generic.Dictionary<int, long> newClientList) {
        var index = GameComponentsLookup.Marketing;
        var component = (MarketingComponent)CreateComponent(index, typeof(MarketingComponent));
        component.clients = newClients;
        component.ClientList = newClientList;
        ReplaceComponent(index, component);
    }

    public void RemoveMarketing() {
        RemoveComponent(GameComponentsLookup.Marketing);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherMarketing;

    public static Entitas.IMatcher<GameEntity> Marketing {
        get {
            if (_matcherMarketing == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Marketing);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMarketing = matcher;
            }

            return _matcherMarketing;
        }
    }
}
