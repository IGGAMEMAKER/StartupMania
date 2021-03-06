//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public PopupComponent popup { get { return (PopupComponent)GetComponent(GameComponentsLookup.Popup); } }
    public bool hasPopup { get { return HasComponent(GameComponentsLookup.Popup); } }

    public void AddPopup(System.Collections.Generic.List<PopupMessage> newPopupMessages) {
        var index = GameComponentsLookup.Popup;
        var component = (PopupComponent)CreateComponent(index, typeof(PopupComponent));
        component.PopupMessages = newPopupMessages;
        AddComponent(index, component);
    }

    public void ReplacePopup(System.Collections.Generic.List<PopupMessage> newPopupMessages) {
        var index = GameComponentsLookup.Popup;
        var component = (PopupComponent)CreateComponent(index, typeof(PopupComponent));
        component.PopupMessages = newPopupMessages;
        ReplaceComponent(index, component);
    }

    public void RemovePopup() {
        RemoveComponent(GameComponentsLookup.Popup);
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

    static Entitas.IMatcher<GameEntity> _matcherPopup;

    public static Entitas.IMatcher<GameEntity> Popup {
        get {
            if (_matcherPopup == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Popup);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherPopup = matcher;
            }

            return _matcherPopup;
        }
    }
}
