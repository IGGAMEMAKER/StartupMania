//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public NicheSegmentsComponent nicheSegments { get { return (NicheSegmentsComponent)GetComponent(GameComponentsLookup.NicheSegments); } }
    public bool hasNicheSegments { get { return HasComponent(GameComponentsLookup.NicheSegments); } }

    public void AddNicheSegments(System.Collections.Generic.List<ProductPositioning> newPositionings) {
        var index = GameComponentsLookup.NicheSegments;
        var component = (NicheSegmentsComponent)CreateComponent(index, typeof(NicheSegmentsComponent));
        component.Positionings = newPositionings;
        AddComponent(index, component);
    }

    public void ReplaceNicheSegments(System.Collections.Generic.List<ProductPositioning> newPositionings) {
        var index = GameComponentsLookup.NicheSegments;
        var component = (NicheSegmentsComponent)CreateComponent(index, typeof(NicheSegmentsComponent));
        component.Positionings = newPositionings;
        ReplaceComponent(index, component);
    }

    public void RemoveNicheSegments() {
        RemoveComponent(GameComponentsLookup.NicheSegments);
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

    static Entitas.IMatcher<GameEntity> _matcherNicheSegments;

    public static Entitas.IMatcher<GameEntity> NicheSegments {
        get {
            if (_matcherNicheSegments == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.NicheSegments);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherNicheSegments = matcher;
            }

            return _matcherNicheSegments;
        }
    }
}