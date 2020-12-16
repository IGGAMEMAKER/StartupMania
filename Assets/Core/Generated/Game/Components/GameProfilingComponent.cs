//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ProfilingComponent profiling { get { return (ProfilingComponent)GetComponent(GameComponentsLookup.Profiling); } }
    public bool hasProfiling { get { return HasComponent(GameComponentsLookup.Profiling); } }

    public void AddProfiling(long newProfilerMilliseconds, System.Text.StringBuilder newMyProfiler, System.Collections.Generic.Dictionary<string, long> newTags) {
        var index = GameComponentsLookup.Profiling;
        var component = (ProfilingComponent)CreateComponent(index, typeof(ProfilingComponent));
        component.ProfilerMilliseconds = newProfilerMilliseconds;
        component.MyProfiler = newMyProfiler;
        component.Tags = newTags;
        AddComponent(index, component);
    }

    public void ReplaceProfiling(long newProfilerMilliseconds, System.Text.StringBuilder newMyProfiler, System.Collections.Generic.Dictionary<string, long> newTags) {
        var index = GameComponentsLookup.Profiling;
        var component = (ProfilingComponent)CreateComponent(index, typeof(ProfilingComponent));
        component.ProfilerMilliseconds = newProfilerMilliseconds;
        component.MyProfiler = newMyProfiler;
        component.Tags = newTags;
        ReplaceComponent(index, component);
    }

    public void RemoveProfiling() {
        RemoveComponent(GameComponentsLookup.Profiling);
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

    static Entitas.IMatcher<GameEntity> _matcherProfiling;

    public static Entitas.IMatcher<GameEntity> Profiling {
        get {
            if (_matcherProfiling == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Profiling);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherProfiling = matcher;
            }

            return _matcherProfiling;
        }
    }
}
