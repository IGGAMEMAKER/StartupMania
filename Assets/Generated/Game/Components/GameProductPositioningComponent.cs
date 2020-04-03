//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ProductPositioningComponent productPositioning { get { return (ProductPositioningComponent)GetComponent(GameComponentsLookup.ProductPositioning); } }
    public bool hasProductPositioning { get { return HasComponent(GameComponentsLookup.ProductPositioning); } }

    public void AddProductPositioning(int newPositioning) {
        var index = GameComponentsLookup.ProductPositioning;
        var component = (ProductPositioningComponent)CreateComponent(index, typeof(ProductPositioningComponent));
        component.Positioning = newPositioning;
        AddComponent(index, component);
    }

    public void ReplaceProductPositioning(int newPositioning) {
        var index = GameComponentsLookup.ProductPositioning;
        var component = (ProductPositioningComponent)CreateComponent(index, typeof(ProductPositioningComponent));
        component.Positioning = newPositioning;
        ReplaceComponent(index, component);
    }

    public void RemoveProductPositioning() {
        RemoveComponent(GameComponentsLookup.ProductPositioning);
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

    static Entitas.IMatcher<GameEntity> _matcherProductPositioning;

    public static Entitas.IMatcher<GameEntity> ProductPositioning {
        get {
            if (_matcherProductPositioning == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ProductPositioning);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherProductPositioning = matcher;
            }

            return _matcherProductPositioning;
        }
    }
}