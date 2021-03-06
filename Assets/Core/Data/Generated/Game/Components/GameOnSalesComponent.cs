//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly OnSalesComponent onSalesComponent = new OnSalesComponent();

    public bool isOnSales {
        get { return HasComponent(GameComponentsLookup.OnSales); }
        set {
            if (value != isOnSales) {
                var index = GameComponentsLookup.OnSales;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : onSalesComponent;

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

    static Entitas.IMatcher<GameEntity> _matcherOnSales;

    public static Entitas.IMatcher<GameEntity> OnSales {
        get {
            if (_matcherOnSales == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.OnSales);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherOnSales = matcher;
            }

            return _matcherOnSales;
        }
    }
}
