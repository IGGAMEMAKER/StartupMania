//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public HumanSkillsComponent humanSkills { get { return (HumanSkillsComponent)GetComponent(GameComponentsLookup.HumanSkills); } }
    public bool hasHumanSkills { get { return HasComponent(GameComponentsLookup.HumanSkills); } }

    public void AddHumanSkills(System.Collections.Generic.Dictionary<WorkerRole, int> newRoles, System.Collections.Generic.List<TraitType> newTraits, System.Collections.Generic.Dictionary<NicheType, int> newExpertise) {
        var index = GameComponentsLookup.HumanSkills;
        var component = (HumanSkillsComponent)CreateComponent(index, typeof(HumanSkillsComponent));
        component.Roles = newRoles;
        component.Traits = newTraits;
        component.Expertise = newExpertise;
        AddComponent(index, component);
    }

    public void ReplaceHumanSkills(System.Collections.Generic.Dictionary<WorkerRole, int> newRoles, System.Collections.Generic.List<TraitType> newTraits, System.Collections.Generic.Dictionary<NicheType, int> newExpertise) {
        var index = GameComponentsLookup.HumanSkills;
        var component = (HumanSkillsComponent)CreateComponent(index, typeof(HumanSkillsComponent));
        component.Roles = newRoles;
        component.Traits = newTraits;
        component.Expertise = newExpertise;
        ReplaceComponent(index, component);
    }

    public void RemoveHumanSkills() {
        RemoveComponent(GameComponentsLookup.HumanSkills);
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

    static Entitas.IMatcher<GameEntity> _matcherHumanSkills;

    public static Entitas.IMatcher<GameEntity> HumanSkills {
        get {
            if (_matcherHumanSkills == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.HumanSkills);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherHumanSkills = matcher;
            }

            return _matcherHumanSkills;
        }
    }
}
