//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ProductTargetAudienceComponent productTargetAudience { get { return (ProductTargetAudienceComponent)GetComponent(GameComponentsLookup.ProductTargetAudience); } }
    public bool hasProductTargetAudience { get { return HasComponent(GameComponentsLookup.ProductTargetAudience); } }

    public void AddProductTargetAudience(int newSegmentId) {
        var index = GameComponentsLookup.ProductTargetAudience;
        var component = (ProductTargetAudienceComponent)CreateComponent(index, typeof(ProductTargetAudienceComponent));
        component.SegmentId = newSegmentId;
        AddComponent(index, component);
    }

    public void ReplaceProductTargetAudience(int newSegmentId) {
        var index = GameComponentsLookup.ProductTargetAudience;
        var component = (ProductTargetAudienceComponent)CreateComponent(index, typeof(ProductTargetAudienceComponent));
        component.SegmentId = newSegmentId;
        ReplaceComponent(index, component);
    }

    public void RemoveProductTargetAudience() {
        RemoveComponent(GameComponentsLookup.ProductTargetAudience);
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

    static Entitas.IMatcher<GameEntity> _matcherProductTargetAudience;

    public static Entitas.IMatcher<GameEntity> ProductTargetAudience {
        get {
            if (_matcherProductTargetAudience == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ProductTargetAudience);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherProductTargetAudience = matcher;
            }

            return _matcherProductTargetAudience;
        }
    }
}