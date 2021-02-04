using System.Collections;
using UnityEngine;
namespace PositionerDemo
{
    public class MotionController
    {
        private float motionSpeedUpVelocity = 5f;
        Motion actualMotion;
        private KeyCode skipKeyCode = KeyCode.Space;
        private KeyCode fastForwardKeyCode = KeyCode.W;
        private bool isPresing = false;
        private bool isFastForwarding = false;
        private float pressedTime = 0;
        private float timeToStarFastForward = 0.5f;
        public bool IsPerforming
        {
            get
            {
                if (actualMotion != null)
                {
                    return actualMotion.isPerforming;
                }
                else
                {
                    return false;
                }
            }

            private set
            {
                IsPerforming = value;
            }
        }

        public MotionController()
        {

        }

        public void SetUpMotion(Motion actualMotion)
        {
            //actualMotion.SetSkipeableMode(true);
            if (this.actualMotion == null)
            {
                this.actualMotion = actualMotion;
                return;
            }
            if (this.actualMotion.isPerforming == false)
            {
                this.actualMotion = actualMotion;
            }
            else
            {
                Debug.Log("CANT CHANGE IS PERFORMING");
            }
        }

        public void TryReproduceMotion()
        {
            if (actualMotion==null) return;

            if (!actualMotion.isPerforming)
            {
                actualMotion.OnTryReproduceAnimotion();
                if (actualMotion.isPerforming)
                {
                    actualMotion.coroutineMono.StartCoroutine(SpeedUpOnButtonPressCoroutine());
                }
            }
            else
            {
                Debug.Log("Is Performing animation");
            }
        }
        
        private IEnumerator SpeedUpOnButtonPressCoroutine()
        {
            isFastForwarding = false;
            pressedTime = 0;
            isPresing = false;

            while (actualMotion.isPerforming)
            {
                if (actualMotion == null) yield break;

                if (Input.GetKey(fastForwardKeyCode))
                {
                    isPresing = true;
                }
                else if (Input.GetKeyUp(fastForwardKeyCode))
                {
                    isPresing = false;
                    pressedTime = 0;
                    if (isFastForwarding)
                    {
                        SetNormalSpeedMotion();
                        isFastForwarding = false;
                    }
                }

                if (actualMotion.isPerforming)
                {
                    if (isPresing && isFastForwarding == false)
                    {
                        pressedTime += Time.deltaTime;
                        if (pressedTime >= timeToStarFastForward)
                        {
                            isFastForwarding = true;
                            SpeedUpMotion();
                        }
                    }
                }

                yield return null;
            }
        }

        private void SpeedUpMotion()
        {
            actualMotion.SpeedUpMotion(motionSpeedUpVelocity);
        }

        private void SetNormalSpeedMotion()
        {
            actualMotion.SetNormalSpeedMotion();
        }

        private IEnumerator EndMotionOnButtonPress()
        {
            bool done = false;
            while (!done)
            {
                if (actualMotion.isPerforming == false)
                {
                    done = true;
                    yield break;
                }
                if (Input.GetKeyDown(skipKeyCode))
                {
                    if (actualMotion.isPerforming == true)
                    {
                        actualMotion.isCancel = true;
                        done = true;
                        actualMotion.OnMotionSkip();

                        yield break;
                    }
                }
                yield return null;
            }
            yield return null;
        }
    }
}