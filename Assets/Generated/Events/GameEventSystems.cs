//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventSystemsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class GameEventSystems : Feature {

    public GameEventSystems(Contexts contexts) {
        Add(new CompanyEventSystem(contexts)); // priority: 0
        Add(new AnyCompanyEventSystem(contexts)); // priority: 0
        Add(new CompanyResourceEventSystem(contexts)); // priority: 0
        Add(new CrunchingEventSystem(contexts)); // priority: 0
        Add(new AnyDateEventSystem(contexts)); // priority: 0
        Add(new DevelopmentFocusEventSystem(contexts)); // priority: 0
        Add(new FinanceEventSystem(contexts)); // priority: 0
        Add(new MarketingEventSystem(contexts)); // priority: 0
        Add(new MenuEventSystem(contexts)); // priority: 0
        Add(new AnyNotificationsEventSystem(contexts)); // priority: 0
        Add(new ProductEventSystem(contexts)); // priority: 0
        Add(new ShareholdersEventSystem(contexts)); // priority: 0
        Add(new AnyShareholdersEventSystem(contexts)); // priority: 0
        Add(new TargetingEventSystem(contexts)); // priority: 0
        Add(new TeamEventSystem(contexts)); // priority: 0
        Add(new TechnologyLeaderEventSystem(contexts)); // priority: 0
        Add(new AnyTechnologyLeaderEventSystem(contexts)); // priority: 0
    }
}
