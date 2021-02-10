using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikzeerGame.Animotion;
namespace CommandPatternActions
{
    public static class InvokerAnimotion
    {
        private static List<Animotion> motionsToExecute = new List<Animotion>();
        private static bool isExecuting;
        private static AnimotionController motionController = new AnimotionController();
        static bool logOn = false;
        public static void AddNewMotion(Animotion motion)
        {
            motionsToExecute.Add(motion);
            if (logOn) Debug.Log("NEW MOTION ADDED");
        }

        public static void RemoveMotion(Animotion motion)
        {
            motionsToExecute.Remove(motion);
            if (logOn) Debug.Log("MOTION REMOVED");
        }

        public static void StartExecution(MonoBehaviour dummy)
        {
            if (isExecuting)
            {
                if (logOn) Debug.Log("IS EXECUTION");
                return;
            }

            if (motionsToExecute.Count == 0)
            {
                if (logOn) Debug.Log("NO MOTION");
                return;
            }
            dummy.StartCoroutine(ExecuteMotion());
        }

        public static IEnumerator ExecuteMotion()
        {
            isExecuting = true;

            while (isExecuting)
            {
                // SI ESTA EJECUTANDO ENTONCES ESPERAMOS
                if (motionController.IsPerforming)
                {
                    if (logOn) Debug.Log("IS PERFORMING");
                    yield return null;
                }
                else
                {
                    // SI NO HAY MAS MOTIONS QUE EXECUTE ENTONCES TERMINAMOS
                    if (motionsToExecute.Count == 0)
                    {
                        if (logOn) Debug.Log("ACTUAL MOTION HAS FINISH");
                        isExecuting = false;
                    }
                    else
                    {
                        // SI NO ESTA EJECUTANDO, EJECUTAMOS
                        Animotion motionToExecute = motionsToExecute[0];
                        ReproduceMotion(motionToExecute);
                        RemoveMotion(motionsToExecute[0]);
                        yield return null;
                    }
                }
            }
            if (logOn) Debug.Log("ALL MOTIONS FINISH");

            if (motionsToExecute.Count > 0)
            {
                if (logOn) Debug.Log("SE AGREGO UNA MOTION DESPUES DE FINALIZAR");
            }
        }

        private static void ReproduceMotion(Animotion motion)
        {
            motionController.SetUpMotion(motion);
            motionController.TryReproduceMotion();
        }

        public static bool IsExecuting()
        {
            return isExecuting;
        }
    }
}