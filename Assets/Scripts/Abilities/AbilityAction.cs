using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PositionerDemo
{
    public abstract class AbilityAction : Ability
    {
        private const int ACTIONPOINTSTATID = 4;
        private int id;
        public int ID { get { return id; } private set { id = value; } }
        public ABILITYEXECUTIONSTATUS actionStatus { get; set; }
        public IOcuppy performerIOcuppy { get; private set; }
        private int actionPointsRequired; // Cantidad de Action Points que requiera la accion para ejecutarse
        private ABILITYTYPE abilityType;
        public ABILITYTYPE AbilityType { get { return abilityType; } private set { abilityType = value; } }
        public List<AbilityModifier> abilityModifier { get; private set; }

        public AbilityAction(int ID, IOcuppy performerIOcuppy, int actionPointsRequired, ABILITYTYPE abilityType)
        {
            this.performerIOcuppy = performerIOcuppy;
            this.ID = ID;
            this.actionPointsRequired = actionPointsRequired;
            abilityModifier = new List<AbilityModifier>();
            TurnManager.OnChangeTurn += OnResetActionExecution;
            this.abilityType = abilityType;
        }

        public abstract void OnResetActionExecution();//-> este es un evento el cual se va a suscribir al TurnChange.?invoke(OnResetActionExecution()) para poner el estado en no ejecutado
        public abstract bool OnTryEnter();
        public abstract bool OnTryExecute();
        private void StartActionModifierCheck() // Cuando empezamos a ejecuta esta accion y chequeamos los modificadores de accion que se ejecutan al inicio de la ejecucion de la accion StartActionModifier
        {
            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
            abilityModifier.OrderBy(c => c.ModifierExecutionOrder);
            for (int i = 0; i < abilityModifier.Count; i++)
            {
                if (abilityModifier[i].executionTime == ABILITYMODIFIEREXECUTIONTIME.EARLY)
                {
                    abilityModifier[i].Execute(this);
                    if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                }
            }
        }
        public abstract void OnStartExecute();
        public abstract void Execute();
        private void EndActionModifierCheck()// Cuando terminamos de ejecutar esta accion y chequeamos los modificadores de accion que se ejecutan al final de la ejecucion de la accion EndActionModifier
        {
            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;

            abilityModifier.OrderBy(c => c.ModifierExecutionOrder);

            for (int i = 0; i < abilityModifier.Count; i++)
            {
                if (abilityModifier[i].executionTime == ABILITYMODIFIEREXECUTIONTIME.LATE)
                {
                    abilityModifier[i].Execute(this);
                }
            }
        }
        public abstract void OnEndExecute();

        public void Perform()
        {
            //OnResetActionExecution();
            if (OnTryExecute())
            {
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                StartActionModifierCheck();
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                OnStartExecute();
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                Execute();
                EndActionModifierCheck();
                RestActionPoints();
                OnEndExecute();
            }
        }

        //resta la cantidad de action points requeridos por la accion
        private void RestActionPoints()
        {
            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;

            if (performerIOcuppy.Stats.ContainsKey(ACTIONPOINTSTATID))
            {
                StatModification statModification = new StatModification(performerIOcuppy, performerIOcuppy.Stats[ACTIONPOINTSTATID], ACTIONPOINTSTATID, -actionPointsRequired, STATMODIFIERTYPE.NERF);
                performerIOcuppy.Stats[ACTIONPOINTSTATID].AddStatModifier(statModification);
                performerIOcuppy.Stats[ACTIONPOINTSTATID].ApplyModifications();

                //if (performerIOcuppy.Stats[ACTIONPOINTSTATID].ActualStatValue == 0)
                //{
                //    Debug.Log("Llegue a Zero Action Points de Habilidad ACA DEBERIA ACTIVAR EL ON CHANGE TURN SI FUERA UN PLAYER....");
                //}
            }          
        }

        public int GetActionPointsRequiredToUseAbility()
        {
            return actionPointsRequired;
        }

        public void AddAbilityModifier(AbilityModifier modifier)
        {
            abilityModifier.Add(modifier);
        }

        public void RemoveAbilityModifier(AbilityModifier modifier)
        {
            abilityModifier.Remove(modifier);
        }

    }
}

namespace MikzeerGameNotas
{
    //public class UseCardAbility : AbilityAction
    //{
    //    private const int specificID = 8;
    //    private int amount = 0;
    //    private Player player;
    //    Card card;
    //    IUseCardCommnad useCardCommand;

    //    ICardTarget cardTarget;

    //    public List<Tile2D> posibleTargetTiles { get; protected set; }
    //    public Tile2D selectedTargetTile;


    //    public UseCardAbility(Player player) : base(specificID)
    //    {
    //        this.player = player;
    //        SetActionPointsRequired(amount);
    //        SetPerformerAPActor(player);
    //    }

    //    public void Set(Card card)
    //    {
    //        this.card = card;
    //    }

    //    public void SetTarget(ICardTarget cardTarget)
    //    {
    //        this.cardTarget = cardTarget;
    //    }

    //    public override void OnResetActionExecution()
    //    {

    //    }

    //    public override bool OnTryExecute()
    //    {
    //        // 1- QUE LA CARTA TENGA UN TARGET
    //        if (!card.DoIHaveTarget())
    //        {
    //            return false;
    //        }

    //        return true;
    //    }

    //    public override void StartActionModifierCheck()
    //    {

    //    }

    //    public override void OnStartExecute()
    //    {

    //    }

    //    public override void Execute()
    //    {
    //        if (actionStatus == ACTIONEXECUTIONSTATUS.CANCELED)
    //        {
    //            Debug.Log("UseCardAbility CANCEL");
    //            return;
    //        }

    //        useCardCommand = new IUseCardCommnad(player, card);

    //        Invoker.AddNewCommand(useCardCommand);
    //        Invoker.ExecuteCommands(null);

    //        //actionStatus = ACTIONEXECUTIONSTATUS.STARTED;
    //        //dummy.StartCoroutine(CheckExecution());
    //    }

    //    public override void EndActionModifierCheck()
    //    {

    //    }

    //    public override void OnEndExecute()
    //    {

    //    }

    //    public IEnumerator CheckExecution()
    //    {
    //        bool hasEnd = false;
    //        do
    //        {
    //            if (useCardCommand.hasFinish)
    //            {
    //                //Debug.Log("Ya termine de Spawnear segunda capa");
    //                actionStatus = ACTIONEXECUTIONSTATUS.EXECUTED;
    //                hasEnd = true;
    //                yield break;
    //            }

    //            //Debug.Log("Todavia no termine");
    //            yield return new WaitForSeconds(0.15f);
    //        } while (!hasEnd);

    //        //Debug.Log("Termine de Spawnear");

    //    }

    //}
}