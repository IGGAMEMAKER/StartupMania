//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly UniversalListenerComponent universalListenerComponent = new UniversalListenerComponent();

    public bool isUniversalListener {
        get { return HasComponent(GameComponentsLookup.UniversalListener); }
        set {
            if (value != isUniversalListener) {
                var index = GameComponentsLookup.UniversalListener;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : universalListenerComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
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

    static Entitas.IMatcher<GameEntity> _matcherUniversalListener;

    public static Entitas.IMatcher<GameEntity> UniversalListener {
        get {
            if (_matcherUniversalListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.UniversalListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherUniversalListener = matcher;
            }

            return _matcherUniversalListener;
        }
    }
}
