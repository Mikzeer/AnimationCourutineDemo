using PositionerDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CommandPatternActions
{
    public static class InvokerMotion
    {
        private static List<PositionerDemo.Motion> motionsToExecute = new List<PositionerDemo.Motion>();
        private static bool isExecuting;
        private static MotionController motionController = new MotionController();
        static bool logOn = false;
        public static void AddNewMotion(PositionerDemo.Motion motion)
        {
            motionsToExecute.Add(motion);
            if(logOn)Debug.Log("NEW MOTION ADDED");
        }

        public static void RemoveMotion(PositionerDemo.Motion motion)
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
                    if(logOn) Debug.Log("IS PERFORMING");
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
                        PositionerDemo.Motion motionToExecute = motionsToExecute[0];
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

        private static void ReproduceMotion(PositionerDemo.Motion motion)
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