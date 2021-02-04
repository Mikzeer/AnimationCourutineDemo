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

        public CombineMotion(MonoBehaviour coroutineMono, int reproductionOrder, List<Motion> motions, List<Configurable> configureAnimotion = null) : base(coroutineMono, reproductionOrder)
        {
            this.motions = motions;
            if (configureAnimotion != null)
            {
                this.configureAnimotion = configureAnimotion;
            }
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
                        if (configureAnimotion[i].configureOrder == actualMotionIndex)
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
                        motions[i].OnTryReproduceAnimotion();
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
                //Debug.Log("PERFORMING");

                if (isCancel)
                {
                    hasAllEnded = true;
                    yield break;
                }
                bool hasThisOrderEnded = false;

                while (!hasThisOrderEnded)
                {
                    hasThisOrderEnded = true;
                    for (int i = motions.Count - 1; i >= 0; i--)
                    {
                        if (motions[i].reproductionOrder == currentPerformingIndex && !motions[i].isPerforming)
                        {
                            motions.RemoveAt(i);
                        }
                        else if (motions[i].reproductionOrder == currentPerformingIndex && motions[i].isPerforming)
                        {
                            hasThisOrderEnded = false;
                        }
                    }

                    if (motions.Count == 0)
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

                if (isCancel)
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

                    for (int i = 0; i < motions.Count; i++)
                    {
                        if (motions[i].reproductionOrder == actualMotionIndex)
                        {
                            motions[i].OnTryReproduceAnimotion();
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
            for (int i = 0; i < motions.Count; i++)
            {
                motions[i].OnMotionSkip();
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

        protected override void CheckMotionAfterEnd()
        {
            OnMotionSkip();
        }

        protected override void SpeedUpMotionOnMotion()
        {
            for (int i = 0; i < motions.Count; i++)
            {
                motions[i].SpeedUpMotion(speed);
                //if (motions[i].reproductionOrder == currentPerformingIndex)
                //{
                //    motions[i].SpeedUpMotion(speed);
                //}
            }
        }

        protected override void SetNormalSpeedInMotion()
        {
            for (int i = 0; i < motions.Count; i++)
            {
                motions[i].SetNormalSpeedMotion();
                //if (motions[i].reproductionOrder == currentPerformingIndex)
                //{
                //    motions[i].SetNormalSpeedMotion();
                //}
            }
        }

    }

}

