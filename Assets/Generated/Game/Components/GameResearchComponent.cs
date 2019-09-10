//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ResearchComponent research { get { return (ResearchComponent)GetComponent(GameComponentsLookup.Research); } }
    public bool hasResearch { get { return HasComponent(GameComponentsLookup.Research); } }

    public void AddResearch(int newLevel) {
        var index = GameComponentsLookup.Research;
        var component = (ResearchComponent)CreateComponent(index, typeof(ResearchComponent));
        component.Level = newLevel;
        AddComponent(index, component);
    }

    public void ReplaceResearch(int newLevel) {
        var index = GameComponentsLookup.Research;
        var component = (ResearchComponent)CreateComponent(index, typeof(ResearchComponent));
        component.Level = newLevel;
        ReplaceComponent(index, component);
    }

    public void RemoveResearch() {
        RemoveComponent(GameComponentsLookup.Research);
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

    static Entitas.IMatcher<GameEntity> _matcherResearch;

    public static Entitas.IMatcher<GameEntity> Research {
        get {
            if (_matcherResearch == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Research);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherResearch = matcher;
            }

            return _matcherResearch;
        }
    }
}
