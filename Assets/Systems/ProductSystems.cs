﻿public class ProductSystems : Feature
{
    public ProductSystems(Contexts contexts) : base("Product Systems")
    {
        Add(new ProductInitializerSystem(contexts));
        //Add(new RegisterProductChanges(contexts));
        Add(new UpgradeProductSystem(contexts));
    }
}
