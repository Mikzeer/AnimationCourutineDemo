using PositionerDemo;
namespace CommandPatternActions
{
    public class IRestActionPointsCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool isRunning { get; set; }
        public bool logInsert { get; set; }
        public IOcuppy performerIOcuppy;
        private int actionPointsRequired;

        public IRestActionPointsCommand(IOcuppy performerIOcuppy, int actionPointsRequired)
        {
            this.performerIOcuppy = performerIOcuppy;
            this.actionPointsRequired = actionPointsRequired;
        }

        public void Execute()
        {
            if (performerIOcuppy.Stats.ContainsKey(STATTYPE.ACTIONPOINTS))
            {
                StatModification statModification = new StatModification(performerIOcuppy, performerIOcuppy.Stats[STATTYPE.ACTIONPOINTS], -actionPointsRequired, STATMODIFIERTYPE.NERF);
                performerIOcuppy.Stats[STATTYPE.ACTIONPOINTS].AddStatModifier(statModification);
                performerIOcuppy.Stats[STATTYPE.ACTIONPOINTS].ApplyModifications();
                //if (performerIOcuppy.Stats[ACTIONPOINTSTATID].ActualStatValue == 0)
                //{
                //    Debug.Log("Llegue a Zero Action Points de Habilidad ACA DEBERIA ACTIVAR EL ON CHANGE TURN SI FUERA UN PLAYER....");
                //}
            }
        }

        public void Unexecute()
        {
            if (performerIOcuppy.Stats.ContainsKey(STATTYPE.ACTIONPOINTS))
            {
                StatModification statModification = new StatModification(performerIOcuppy, performerIOcuppy.Stats[STATTYPE.ACTIONPOINTS], actionPointsRequired, STATMODIFIERTYPE.BUFF);
                performerIOcuppy.Stats[STATTYPE.ACTIONPOINTS].AddStatModifier(statModification);
                performerIOcuppy.Stats[STATTYPE.ACTIONPOINTS].ApplyModifications();
            }
        }
    }
}