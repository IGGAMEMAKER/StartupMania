//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public CompanyListenerComponent companyListener { get { return (CompanyListenerComponent)GetComponent(GameComponentsLookup.CompanyListener); } }
    public bool hasCompanyListener { get { return HasComponent(GameComponentsLookup.CompanyListener); } }

    public void AddCompanyListener(System.Collections.Generic.List<ICompanyListener> newValue) {
        var index = GameComponentsLookup.CompanyListener;
        var component = (CompanyListenerComponent)CreateComponent(index, typeof(CompanyListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceCompanyListener(System.Collections.Generic.List<ICompanyListener> newValue) {
        var index = GameComponentsLookup.CompanyListener;
        var component = (CompanyListenerComponent)CreateComponent(index, typeof(CompanyListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveCompanyListener() {
        RemoveComponent(GameComponentsLookup.CompanyListener);
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

    static Entitas.IMatcher<GameEntity> _matcherCompanyListener;

    public static Entitas.IMatcher<GameEntity> CompanyListener {
        get {
            if (_matcherCompanyListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CompanyListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCompanyListener = matcher;
            }

            return _matcherCompanyListener;
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

    public void AddCompanyListener(ICompanyListener value) {
        var listeners = hasCompanyListener
            ? companyListener.value
            : new System.Collections.Generic.List<ICompanyListener>();
        listeners.Add(value);
        ReplaceCompanyListener(listeners);
    }

    public void RemoveCompanyListener(ICompanyListener value, bool removeComponentWhenEmpty = true) {
        var listeners = companyListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveCompanyListener();
        } else {
            ReplaceCompanyListener(listeners);
        }
    }
}
