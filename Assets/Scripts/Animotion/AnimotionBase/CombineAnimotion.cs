using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace MikzeerGame.Animotion
{
    public class CombineAnimotion : Animotion
    {
        public List<Animotion> animotions;
        public List<ConfigurableMotion> configureAnimotion;
        private int actualMotionIndex = 1;
        private int currentPerformingIndex;

        public CombineAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, List<Animotion> motions, List<ConfigurableMotion> configureAnimotion = null) : base(coroutineMono, reproductionOrder)
        {
            this.animotions = motions;
            if (configureAnimotion != null)
            {
                this.configureAnimotion = configureAnimotion;
            }
        }

        protected override void StartMotion()
        {
            if (animotions.Count <= 0) return;

            bool started = false;

            while (started == false)
            {
                if (configureAnimotion != null)
                {
                    for (int i = configureAnimotion.Count - 1; i >= 0; i--)
                    {
                        if (configureAnimotion[i].configureOrder == actualMotionIndex)
                        {
                            configureAnimotion[i].Configure();
                            configureAnimotion.RemoveAt(i);
                            started = true;
                        }
                    }
                }

                for (int i = 0; i < animotions.Count; i++)
                {
                    if (animotions[i].reproductionOrder == actualMotionIndex)
                    {
                        animotions[i].OnTryReproduceAnimotion();
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
                //Debug.Log("PERFORMING COMBINE MOTION REP ORDER: " + currentPerformingIndex);

                if (executionState == ANIMOTIONEXECUTIONSTATE.SKIP)
                {
                    hasAllEnded = true;
                    yield break;
                }
                bool hasThisOrderEnded = false;

                while (!hasThisOrderEnded)
                {
                    //Debug.Log("PERFORMING hasThisOrderEnded REP ORDER: " + currentPerformingIndex);
                    hasThisOrderEnded = true;
                    if (executionState == ANIMOTIONEXECUTIONSTATE.SKIP)
                    {
                        hasAllEnded = true;
                        yield break;
                    }
                    for (int i = animotions.Count - 1; i >= 0; i--)
                    {
                        if (animotions[i].reproductionOrder == currentPerformingIndex)
                        {
                            switch (animotions[i].executionState)
                            {
                                case ANIMOTIONEXECUTIONSTATE.WAIT:
                                    break;
                                case ANIMOTIONEXECUTIONSTATE.SKIP:
                                    Debug.Log("PERFORMING SKIPED REP ORDER: " + animotions[i].reproductionOrder);
                                    break;
                                case ANIMOTIONEXECUTIONSTATE.PERFORMING:
                                    hasThisOrderEnded = false;
                                    break;
                                case ANIMOTIONEXECUTIONSTATE.EXECUTED:
                                    animotions.RemoveAt(i);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (animotions.Count == 0)
                    {
                        hasAllEnded = true;
                        if (configureAnimotion != null && configureAnimotion.Count > 0)
                        {
                            for (int i = configureAnimotion.Count - 1; i >= 0; i--)
                            {
                                configureAnimotion[i].Configure();
                                configureAnimotion.RemoveAt(i);
                            }
                        }
                        yield break;
                    }
                    yield return null;
                }

                if (executionState == ANIMOTIONEXECUTIONSTATE.SKIP)
                {
                    hasAllEnded = true;
                    yield break;
                }

                bool started = false;
                while (started == false)
                {
                    if (configureAnimotion != null)
                    {
                        for (int i = configureAnimotion.Count - 1; i >= 0; i--)
                        {
                            if (configureAnimotion[i].configureOrder == actualMotionIndex)
                            {
                                configureAnimotion[i].Configure();
                                configureAnimotion.RemoveAt(i);
                                started = true;
                            }
                        }
                    }

                    for (int i = 0; i < animotions.Count; i++)
                    {
                        if (animotions[i].reproductionOrder == actualMotionIndex)
                        {
                            animotions[i].OnTryReproduceAnimotion();
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

                currentPerformingIndex = actualMotionIndex;
                actualMotionIndex++;
            }
        }

        public override void OnMotionSkip()
        {
            base.OnMotionSkip();

            for (int i = 0; i < animotions.Count; i++)
            {
                animotions[i].OnMotionSkip();
                //Debug.Log("ON MOTION SKIP REP ORDER " + animotions[i].reproductionOrder);
            }

            if (configureAnimotion != null)
            {
                for (int i = 0; i < configureAnimotion.Count; i++)
                {
                    if (configureAnimotion[i].isForced)
                    {
                        configureAnimotion[i].Configure();
                    }
                }
            }
        }

        protected override void OnMotionEnd()
        {
            OnMotionSkip();
        }

        protected override void SpeedUpMotionOnMotion()
        {
            for (int i = 0; i < animotions.Count; i++)
            {
                animotions[i].SpeedUpMotion(speed);
            }
        }

        protected override void SetNormalSpeedInMotion()
        {
            for (int i = 0; i < animotions.Count; i++)
            {
                animotions[i].SetNormalSpeedMotion();
            }
        }
    }
}