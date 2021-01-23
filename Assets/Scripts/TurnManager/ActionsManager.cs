using PositionerDemo;
using System.Collections.Generic;

public class ActionsManager
{

    public void IncrementPlayerActions(Player player, int acAmount)
    {
        //acAmount = 1;
        //  LE DOY UN PUNTO AL PLAYER
        player.ResetActionPoints(acAmount);
        //  RESETEO TODAS SUS HABILIDADES PARA QUE LAS PUEDA EJECUTAR
        foreach (KeyValuePair<ABILITYTYPE, IAbility> ab in player.Abilities)
        {
            ab.Value.actionStatus = ABILITYEXECUTIONSTATUS.WAIT;
        }
    }

    public void IncrementPlayerUnitsActions(Player player, int acAmount)
    {
        acAmount = 2;
        for (int i = 0; i < player.kimbokoUnits.Count; i++)
        {
            player.kimbokoUnits[i].ResetActionPoints(acAmount);
            foreach (KeyValuePair<ABILITYTYPE, IAbility> ab in player.kimbokoUnits[i].Abilities)
            {
                ab.Value.actionStatus = ABILITYEXECUTIONSTATUS.WAIT;
            }
        }
    }

    public void RestPlayerActions(Player player)
    {
        int acAmount = 0;
        //  LE DOY UN PUNTO AL PLAYER
        player.ResetActionPoints(acAmount);
        //  RESETEO TODAS SUS HABILIDADES PARA QUE LAS PUEDA EJECUTAR
        foreach (KeyValuePair<ABILITYTYPE, IAbility> ab in player.Abilities)
        {
            if (ab.Key == ABILITYTYPE.DIE || ab.Key == ABILITYTYPE.TAKEDAMAGE)
            {
                continue;
            }
            ab.Value.actionStatus = ABILITYEXECUTIONSTATUS.NONEXECUTABLE;
        }
    }

    public void RestPlayerUnitsActions(Player player)
    {
        int acAmount = 0;
        for (int i = 0; i < player.kimbokoUnits.Count; i++)
        {
            player.kimbokoUnits[i].ResetActionPoints(acAmount);
            foreach (KeyValuePair<ABILITYTYPE, IAbility> ab in player.kimbokoUnits[i].Abilities)
            {
                if (ab.Key == ABILITYTYPE.DIE || ab.Key == ABILITYTYPE.TAKEDAMAGE)
                {
                    continue;
                }
                ab.Value.actionStatus = ABILITYEXECUTIONSTATUS.NONEXECUTABLE;
            }
        }
    }

    public bool DoesThePlayerHaveActionToExecute(Player player)
    {
        return player.GetCurrentActionPoints() > 0;
    }

    public bool DoesThePlayerUnitsHaveActionsToExecute(Player player)
    {
        for (int i = 0; i < player.kimbokoUnits.Count; i++)
        {
            if (player.kimbokoUnits[i].GetCurrentActionPoints() > 0)
            {
                return true;
            }
        }
        return false;
    }

}