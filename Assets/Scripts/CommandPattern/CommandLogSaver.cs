using System.Collections.Generic;
using UnityEngine;

namespace CommandPatternActions
{
    public static class CommandLogSaver
    {
        public static List<ICommand> allExecutedCommands = new List<ICommand>();
        private static Stack<ICommand> executedCmd = new Stack<ICommand>();

        public static void AddCommandToUndo(ICommand executedCommand)
        {
            allExecutedCommands.Add(executedCommand);
            executedCmd.Push(executedCommand);
        }

        public static void UndoLastCommand()
        {
            if (executedCmd.Count > 0)
            {
                ICommand cmdAux = executedCmd.Pop();
                cmdAux.Unexecute();
            }
            else
            {
                Debug.Log("Yo no hay mas acciones en la lista");
            }
        }
    }
}