using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PositionerDemo
{
    public class CombineMotion : Motion
    {
        public List<Motion> motions;
        public List<Configurable> configureAnimotion;
        private int actualMotionIndex = 1;
        private int currentPerformingIndex;

        public List<Motion> notEndedMotions;

        public CombineMotion(MonoBehaviour coroutineMono, int reproductionOrder, List<Motion> motions, List<Configurable> configureAnimotion = null) : base(coroutineMono, reproductionOrder)
        {
            this.motions = motions;
            if (configureAnimotion != null)
            {
                this.configureAnimotion = configureAnimotion;
            }

            notEndedMotions = new List<Motion>();
        }

        protected override void StartMotion()
        {
            if (motions.Count <= 0) return;

            bool started = false;

            while (started == false)
            {
                if (configureAnimotion != null)
                {
                    for (int i = configureAnimotion.Count - 1; i >= 0; i--)
                    {
                        if (configureAnimotion[i].configureOrder == currentPerformingIndex)
                        {
                            configureAnimotion[i].Configure();
                            configureAnimotion.RemoveAt(i);
                            started = true;
                        }
                    }
                }

                for (int i = 0; i < motions.Count; i++)
                {
                    if (motions[i].reproductionOrder == actualMotionIndex)
                    {
                        motions[i].CheckMotionBeforeStart();
                        started = true;
                    }
                }

                if (started == false)
                {
                    currentPerformingIndex = actualMotionIndex;
                    actualMotionIndex++;
                }
            }

            currentPerformingIndex = actualMotionIndex;
            actualMotionIndex++;
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            bool hasAllEnded = false;
            while (!hasAllEnded)
            {
                //Debug.Log("Chek FINISH currentPerformingIndex " + currentPerformingIndex + " actualMotionIndex " + actualMotionIndex);
                if (cancel)
                {
                    hasAllEnded = true;
                    yield break;
                }

                if (configureAnimotion != null && configureAnimotion.Count > 0)
                {                   
                    for (int i = configureAnimotion.Count - 1; i >= 0; i--)
                    {
                        if (configureAnimotion[i].configureOrder == currentPerformingIndex)
                        {
                            Debug.Log("ENTER CONFIGURABLE ");
                            configureAnimotion[i].Configure();
                            configureAnimotion.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    //Debug.Log("No Configure Animotion");
                }

                bool hasMatch = false;
                int totalReproducingAnimotion = 0;
                int totalEndedReproducingAnimotion = 0;
                // recorro todas las motions
                // segun el index de lo que deberia estar ejecutandose es lo que voy a revisar si se termino, sino, hago un yield return null
                for (int i = motions.Count - 1; i >= 0; i--)
                {
                    //Debug.Log("Enter Check Motions " + currentPerformingIndex + " Motions count " + motions.Count);

                    if (motions[i].reproductionOrder == currentPerformingIndex && !motions[i].performing)
                    {
                        //Debug.Log("Motion End Perform " + currentPerformingIndex);
                        if (motions[i].hasEnded)
                        {
                            notEndedMotions.Add(motions[i]);
                        }

                        motions.RemoveAt(i);
                        totalReproducingAnimotion++;
                        totalEndedReproducingAnimotion++;
                    }
                    else if (motions[i].reproductionOrder == currentPerformingIndex && motions[i].performing)
                    {
                        //Debug.Log("Motion NOT ENDED Perform " + currentPerformingIndex);
                        totalReproducingAnimotion++;
                        hasMatch = true;
                    }
                }

                // Si todos los animotion de ese index estaban finalizados, entonces tenemos un match
                if (totalReproducingAnimotion == totalEndedReproducingAnimotion && totalEndedReproducingAnimotion > 0)
                {
                    //Debug.Log("HAS MATCH ");

                    hasMatch = true;

                    if (motions.Count == 0)
                    {
                        if (notEndedMotions.Count > 0)
                        {

                        }

                        //Debug.Log("HAS MATCH + NO MOTIONS");

                        hasAllEnded = true;

                        if (configureAnimotion != null && configureAnimotion.Count > 0)
                        {
                            //Debug.Log("configureAnimotion.Count " + configureAnimotion.Count);
                            for (int i = configureAnimotion.Count -1 ; i >= 0; i--)
                            {
                                //Debug.Log("ENTER CONFIGURABLE ");
                                configureAnimotion[i].Configure();
                                configureAnimotion.RemoveAt(i);
                            }
                        }


                        yield break;
                    }
                }

                // si ya no quedan mas motions es que ya estan todas finalizadas
                if (motions.Count == 0)
                {
                    //Debug.Log("NO MOTIONS ");

                    hasAllEnded = true;

                    if (configureAnimotion != null && configureAnimotion.Count > 0)
                    {
                        for (int i = configureAnimotion.Count - 1; i >= 0; i--)
                        {
                            Debug.Log("ENTER CONFIGURABLE ");
                            configureAnimotion[i].Configure();
                            configureAnimotion.RemoveAt(i);
                        }
                    }


                    yield break;
                }

                if (hasMatch == false)
                {
                    Debug.Log("NO MATCH ");


                    bool started = false;

                    currentPerformingIndex = actualMotionIndex;
                    actualMotionIndex++;

                    while (started == false)
                    {

                        if (configureAnimotion != null)
                        {
                            for (int i = configureAnimotion.Count - 1; i >= 0; i--)
                            {
                                if (configureAnimotion[i].configureOrder == actualMotionIndex)
                                {
                                    Debug.Log("ENTER CONFIGURABLE ");
                                    //Debug.Log("ENTER CONFIGURABLE ");
                                    configureAnimotion[i].Configure();
                                    configureAnimotion.RemoveAt(i);
                                }
                            }
                        }

                        for (int i = 0; i < motions.Count; i++)
                        {
                            if (motions[i].reproductionOrder == actualMotionIndex)
                            {
                                motions[i].CheckMotionBeforeStart();
                                started = true;
                            }
                        }

                        if (started == false)
                        {
                            currentPerformingIndex = actualMotionIndex;
                            actualMotionIndex++;
                        }

                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }

        public override void OnMotionSkip()
        {
            for (int i = 0; i < motions.Count; i++)
            {
                motions[i].OnMotionSkip();
            }

            for (int i = 0; i < notEndedMotions.Count; i++)
            {
                notEndedMotions[i].OnMotionSkip();
            }
        }

        protected override void CheckMotionAfterEnd()
        {
            //Debug.Log("ENDDDDD");
            OnMotionSkip();
        }

        protected override void SpeedUpMotionOnMotion()
        {
            //Debug.Log("Speed UP");
            for (int i = 0; i < motions.Count; i++)
            {
                if (motions[i].reproductionOrder == currentPerformingIndex)
                {
                    motions[i].SpeedUpMotion(speed);
                }
            }
        }

        protected override void SetNormalSpeedInMotion()
        {
            //Debug.Log("NORMAL SPEED");
            for (int i = 0; i < motions.Count; i++)
            {
                if (motions[i].reproductionOrder == currentPerformingIndex)
                {
                    motions[i].SetNormalSpeedMotion();
                }
            }
        }

    }

}

