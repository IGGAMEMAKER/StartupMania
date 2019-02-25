//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public AnyProductListenerComponent anyProductListener { get { return (AnyProductListenerComponent)GetComponent(GameComponentsLookup.AnyProductListener); } }
    public bool hasAnyProductListener { get { return HasComponent(GameComponentsLookup.AnyProductListener); } }

    public void AddAnyProductListener(System.Collections.Generic.List<IAnyProductListener> newValue) {
        var index = GameComponentsLookup.AnyProductListener;
        var component = (AnyProductListenerComponent)CreateComponent(index, typeof(AnyProductListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAnyProductListener(System.Collections.Generic.List<IAnyProductListener> newValue) {
        var index = GameComponentsLookup.AnyProductListener;
        var component = (AnyProductListenerComponent)CreateComponent(index, typeof(AnyProductListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAnyProductListener() {
        RemoveComponent(GameComponentsLookup.AnyProductListener);
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

    static Entitas.IMatcher<GameEntity> _matcherAnyProductListener;

    public static Entitas.IMatcher<GameEntity> AnyProductListener {
        get {
            if (_matcherAnyProductListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AnyProductListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAnyProductListener = matcher;
            }

            return _matcherAnyProductListener;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public void AddAnyProductListener(IAnyProductListener value) {
        var listeners = hasAnyProductListener
            ? anyProductListener.value
            : new System.Collections.Generic.List<IAnyProductListener>();
        listeners.Add(value);
        ReplaceAnyProductListener(listeners);
    }

    public void RemoveAnyProductListener(IAnyProductListener value, bool removeComponentWhenEmpty = true) {
        var listeners = anyProductListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveAnyProductListener();
        } else {
            ReplaceAnyProductListener(listeners);
        }
    }
}
