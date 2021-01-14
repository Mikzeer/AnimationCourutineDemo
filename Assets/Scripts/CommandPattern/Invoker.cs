using System.Collections.Generic;

namespace CommandPatternActions
{
    public static class Invoker
    {
        private static List<ICommand> commandsToExecute = new List<ICommand>();

        public static void AddNewCommand(ICommand command)
        {
            commandsToExecute.Add(command);
        }

        public static void RemoveCommand(ICommand command)
        {
            commandsToExecute.Remove(command);
        }

        public static void ExecuteCommands()
        {
            for (int i = 0; i < commandsToExecute.Count; i++)
            {
                switch (commandsToExecute[i].executionState)
                {
                    case COMMANDEXECUTINSTATE.WAITFOREXECUTION:
                        commandsToExecute[i].Execute();
                        break;
                    case COMMANDEXECUTINSTATE.INEXECUTION: // NO ESTA ESPERANDO Y SE ESTA EJECUTANDO... POR LAS DUDAS
                        continue;
                    case COMMANDEXECUTINSTATE.FINISH:
                        if (commandsToExecute[i].logInsert == true) CommandLogSaver.AddCommandToUndo(commandsToExecute[i]);
                        commandsToExecute.Remove(commandsToExecute[i]);                        
                        i--;
                        break;
                    case COMMANDEXECUTINSTATE.ABORT:
                        commandsToExecute[i].Unexecute();
                        commandsToExecute.Remove(commandsToExecute[i]);
                        i--;
                        break;
                    case COMMANDEXECUTINSTATE.CANCEL:
                        commandsToExecute[i].Unexecute();
                        commandsToExecute.Remove(commandsToExecute[i]);
                        i--;
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}