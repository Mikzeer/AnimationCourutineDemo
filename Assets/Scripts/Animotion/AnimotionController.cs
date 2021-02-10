using UnityEngine;
using System.Collections;

namespace MikzeerGame.Animotion
{
    public class AnimotionController
    {
        private float motionSpeedUpVelocity = 5f;
        Animotion actualMotion;
        private bool canSkip = true;
        private bool canFastForward = true;
        private KeyCode skipKeyCode = KeyCode.Space;
        private KeyCode fastForwardKeyCode = KeyCode.F;
        private bool isPressing = false;
        private bool isFastForwarding = false;
        private float pressedTime = 0;
        private float timeToStarFastForward = 0.5f;
        public bool IsPerforming
        {
            get
            {
                if (actualMotion != null)
                {
                    return actualMotion.executionState == ANIMOTIONEXECUTIONSTATE.PERFORMING;
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

        IEnumerator speedUpCoroutine;
        IEnumerator skipCoroutine;
        public void SetUpMotion(Animotion actualMotion)
        {
            if (this.actualMotion == null)
            {
                this.actualMotion = actualMotion;
                return;
            }
            if (this.actualMotion.executionState != ANIMOTIONEXECUTIONSTATE.PERFORMING)
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
            if (actualMotion == null) return;

            if (actualMotion.executionState != ANIMOTIONEXECUTIONSTATE.PERFORMING)
            {
                actualMotion.OnTryReproduceAnimotion();
                if (actualMotion.executionState == ANIMOTIONEXECUTIONSTATE.PERFORMING)
                {
                    if (canFastForward)
                    {
                        if (speedUpCoroutine == null)
                        {
                            speedUpCoroutine = SpeedUpOnButtonPressCoroutine();
                            actualMotion.coroutineMono.StartCoroutine(speedUpCoroutine);
                        }
                        else
                        {
                            actualMotion.coroutineMono.StopCoroutine(speedUpCoroutine);
                            speedUpCoroutine = SpeedUpOnButtonPressCoroutine();
                            actualMotion.coroutineMono.StartCoroutine(speedUpCoroutine);
                        }
                    }


                    if (canSkip)
                    {
                        if (skipCoroutine == null)
                        {
                            skipCoroutine = EndMotionOnButtonPress();
                            actualMotion.coroutineMono.StartCoroutine(skipCoroutine);
                        }
                        else
                        {
                            actualMotion.coroutineMono.StopCoroutine(skipCoroutine);
                            skipCoroutine = EndMotionOnButtonPress();
                            actualMotion.coroutineMono.StartCoroutine(skipCoroutine);
                        }
                    }


                    //if (canFastForward)
                    //{
                    //    actualMotion.coroutineMono.StartCoroutine(SpeedUpOnButtonPressCoroutine());
                    //}

                    //if (canSkip)
                    //{
                    //    actualMotion.coroutineMono.StartCoroutine(EndMotionOnButtonPress());
                    //}

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
            isPressing = false;

            while (actualMotion.executionState == ANIMOTIONEXECUTIONSTATE.PERFORMING)
            {
                if (actualMotion == null) yield break;

                if (Input.GetKey(fastForwardKeyCode))
                {
                    isPressing = true;
                }
                else if (Input.GetKeyUp(fastForwardKeyCode))
                {
                    isPressing = false;
                    pressedTime = 0;
                    if (isFastForwarding)
                    {
                        SetNormalSpeedMotion();
                        isFastForwarding = false;
                    }
                }

                if (actualMotion.executionState == ANIMOTIONEXECUTIONSTATE.PERFORMING)
                {
                    if (isPressing && isFastForwarding == false)
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

        private IEnumerator EndMotionOnButtonPress()
        {
            bool done = false;
            while (!done)
            {
                if (actualMotion.executionState != ANIMOTIONEXECUTIONSTATE.PERFORMING)
                {
                    Debug.Log("ACTUAL MOTION IS NOT PERFORMING");
                    done = true;
                    yield break;
                }
                if (Input.GetKeyDown(skipKeyCode))
                {
                    if (actualMotion.executionState == ANIMOTIONEXECUTIONSTATE.PERFORMING)
                    {
                        Debug.Log("ACTUAL MOTION IS SKIPED");
                        done = true;
                        // ESTO LO HACEMOS POR QUE SI TENEMOS VARIAS MOTION A REPRODUCIR NOS VA A TOMAR COMO QUE ESTAMOS SKIPEANDO TODAS EN EL MISMO FRAME
                        // ASI QUE DE ESTA MANERA ESPERAMOS UN FRAME Y LUEGO SKIPEAMOS
                        // ESTO SE DEBE AL GET KEY DOWN QUE RETURNA TRUE EL FRAME QUE SE PRESIONA...
                        yield return null;

                        actualMotion.OnMotionSkip();

                        EndMotion();


                        yield break;
                    }
                }
                yield return null;
            }
            yield return null;
        }

        private void EndMotion()
        {
            // A VERGA
            if (speedUpCoroutine != null)
            {
                actualMotion.coroutineMono.StopCoroutine(speedUpCoroutine);
            }

            if (skipCoroutine != null)
            {
                actualMotion.coroutineMono.StopCoroutine(skipCoroutine);
            }
            skipCoroutine = null;
            speedUpCoroutine = null;
            actualMotion = null;
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