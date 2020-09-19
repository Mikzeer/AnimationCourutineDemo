using System.Collections;
using UnityEngine;
namespace PositionerDemo
{
    public class MotionController
    {
        private float motionSpeedUpVelocity = 5f;

        Motion actualMotion;

        private KeyCode fastForwardKeyCode = KeyCode.W;
        private bool isPresing = false;
        private bool isFastForwarding = false;
        private float pressedTime = 0;
        private float timeToStarFastForward = 0.5f;

        public MotionController()
        {

        }

        public MotionController(Motion actualMotion)
        {
            this.actualMotion = actualMotion;
        }

        public void SetUpMotion(Motion actualMotion)
        {
            if (this.actualMotion == null)
            {
                this.actualMotion = actualMotion;
                return;
            }
            if (this.actualMotion.performing == false)
            {
                this.actualMotion = actualMotion;
            }
            else
            {
                Debug.Log("CANT CHANGE IS PERFORMING");
            }
        }

        public bool isPerforming
        {
            get
            {
                if (actualMotion != null)
                {
                    return actualMotion.performing;
                }
                else
                {
                    return false;
                }
            }

            private set
            {
                isPerforming = value;
            }
        }

        public void TryReproduceMotion()
        {
            if (actualMotion==null) return;

            if (actualMotion.CheckCorrectInput())
            {
                if (!actualMotion.performing)
                {
                    actualMotion.CheckMotionBeforeStart();
                    if (actualMotion.performing)
                    {
                        actualMotion.coroutineMono.StartCoroutine(SpeedUpOnButtonPressCoroutine());
                    }
                }
                else
                {
                    Debug.Log("Is Performing animation");
                }
            }
        }
        
        private IEnumerator SpeedUpOnButtonPressCoroutine()
        {
            isFastForwarding = false;
            pressedTime = 0;
            isPresing = false;

            while (actualMotion.performing)
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

                if (actualMotion.performing)
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

    }
}

