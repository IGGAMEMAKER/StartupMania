//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventListenertInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public interface IProductListener {
    void OnProduct(GameEntity entity, int id, string name, Niche niche, int productLevel, int explorationLevel, WorkerGroup team, Assets.Classes.TeamResource resources, int analytics, int experimentCount);
}
