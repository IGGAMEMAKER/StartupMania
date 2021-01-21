//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public CompanyMarketingActivitiesComponent companyMarketingActivities { get { return (CompanyMarketingActivitiesComponent)GetComponent(GameComponentsLookup.CompanyMarketingActivities); } }
    public bool hasCompanyMarketingActivities { get { return HasComponent(GameComponentsLookup.CompanyMarketingActivities); } }

    public void AddCompanyMarketingActivities(System.Collections.Generic.Dictionary<int, long> newChannels) {
        var index = GameComponentsLookup.CompanyMarketingActivities;
        var component = (CompanyMarketingActivitiesComponent)CreateComponent(index, typeof(CompanyMarketingActivitiesComponent));
        component.Channels = newChannels;
        AddComponent(index, component);
    }

    public void ReplaceCompanyMarketingActivities(System.Collections.Generic.Dictionary<int, long> newChannels) {
        var index = GameComponentsLookup.CompanyMarketingActivities;
        var component = (CompanyMarketingActivitiesComponent)CreateComponent(index, typeof(CompanyMarketingActivitiesComponent));
        component.Channels = newChannels;
        ReplaceComponent(index, component);
    }

    public void RemoveCompanyMarketingActivities() {
        RemoveComponent(GameComponentsLookup.CompanyMarketingActivities);
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

    static Entitas.IMatcher<GameEntity> _matcherCompanyMarketingActivities;

    public static Entitas.IMatcher<GameEntity> CompanyMarketingActivities {
        get {
            if (_matcherCompanyMarketingActivities == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CompanyMarketingActivities);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCompanyMarketingActivities = matcher;
            }

            return _matcherCompanyMarketingActivities;
        }
    }
}