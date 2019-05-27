﻿using Assets.Utils;
using Entitas;
using UnityEngine;

public class BaseClass : MonoBehaviour
{
    public GameEntity SelectedCompany
    {
        get
        {
            return ScreenUtils.GetSelectedCompany(GameContext);
        }
    }

    public bool IsMyCompetitor
    {
        get
        {
            bool isNotMyCompany = MyProductEntity.company.Id != SelectedCompany.company.Id;
            
            return SelectedCompany.hasProduct ? SelectedCompany.product.Niche == MyProduct.Niche && isNotMyCompany : false;
        }
    }

    public ScreenMode CurrentScreen
    {
        get
        {
            return ScreenUtils.GetMenu(GameContext).menu.ScreenMode;
        }
    }

    public GameEntity Me
    {
        get
        {
            return GameContext.GetEntities(GameMatcher.Player)[0];
        }
    }

    public GameEntity SelectedHuman
    {
        get
        {
            return ScreenUtils.GetSelectedHuman(GameContext);
        }
    }

    public GameEntity MyProductEntity
    {
        get
        {
            return CompanyUtils.GetPlayerControlledProductCompany(GameContext);
        }
    }

    public bool HasProductCompany
    {
        get
        {
            return MyProductEntity != null;
        }
    }

    public GameEntity MyGroupEntity
    {
        get
        {
            return CompanyUtils.GetPlayerControlledGroupCompany(GameContext);
        }
    }
    
    public GameContext GameContext
    {
        get
        {
            return Contexts.sharedInstance.game;
        }
    }

    public ProductComponent MyProduct
    {
        get
        {
            return MyProductEntity?.product;
        }
    }

    public int CurrentIntDate
    {
        get
        {
            return ScheduleUtils.GetCurrentDate(GameContext);
        }
    }

    public GameEntity GetUniversalListener
    {
        get
        {
            return ScreenUtils.GetMenu(GameContext);
        }
    }



    internal void ToggleIsChosenComponent(bool isChosen)
    {
        gameObject.GetComponent<IsChosenComponent>().Toggle(isChosen);
    }
}
