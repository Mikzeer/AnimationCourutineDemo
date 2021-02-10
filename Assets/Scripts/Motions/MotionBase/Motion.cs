using System.Collections;
using UnityEngine;
namespace PositionerDemo
{
    public abstract class Motion
    {
        #region VARIABLES
        // Orden en el que se deberia reproducir
        public int reproductionOrder { get; private set; }
        private IEnumerator actualMotionCoroutine;
        public MonoBehaviour coroutineMono { get; private set; }

        public bool isCancel { get; set; } = false;
        public bool isPerforming { get; private set; }

        public bool canSkip = true;
        private KeyCode skipKeyCode = KeyCode.Space;
        protected float speed;

        public ANIMOTIONEXECUTIONSTATE executionState { get; private set; } = ANIMOTIONEXECUTIONSTATE.WAIT;

        #endregion

        public Motion(MonoBehaviour coroutineMono, int reproductionOrder)
        {
            this.coroutineMono = coroutineMono;
            this.reproductionOrder = reproductionOrder;
        }

        public void OnTryReproduceAnimotion()
        {
            if (actualMotionCoroutine != null)
            {
                coroutineMono.StopCoroutine(actualMotionCoroutine);
                actualMotionCoroutine = ReproduceMotion();
                coroutineMono.StartCoroutine(actualMotionCoroutine);
            }
            else
            {
                actualMotionCoroutine = ReproduceMotion();
                coroutineMono.StartCoroutine(actualMotionCoroutine);
            }
        }

        protected IEnumerator ReproduceMotion()
        {
            executionState = ANIMOTIONEXECUTIONSTATE.PERFORMING;
            isPerforming = true;
            isCancel = false;

            StartMotion();
            // Este lo ponemos ya que sino en el primer frame estan todos en Idlle entonces no reconoce el cambio y termina toda la funcion
            // TECNICAMENTE DEBERIA ESTAR SOLUCIONADO SI HABILITAMOS EL CODIGO DEL ANIMATED MOTION QUE CHEQUEA ENTRAR EN EL ESTADO ANTES
            // PARA ESO DEBEMOS CORREGIR TANTO LA ANIMATED MOTION COMO TODOS SUS CHILDS PARA QUE TENGAN LA MISMA LOGICA
            // TENER CUIDADO CON IDDLE O TODOS LOS QUE NO REQUIERAN QUE TERMINE POR LAS DUDAS
            yield return null;

            // Esto es para chequear el final de la animacion/tween/sonido/timer presionando un boton skipKeyCode
            if (canSkip)
            {
                coroutineMono.StartCoroutine(EndMotionOnButtonPress());
            }

            // Esperamos a terminar de hacer la animacion y volver a Iddle tanto de ataque como de recibir dano de los enemigos
            yield return CheckPendingRunningMotions();

            OnMotionEnd();

            yield return null;
        }

        /// <summary>
        /// Aca comenzamos con la motion en si, ya sea un trigger de animacion o un tween, un sonido, o iniciar un timer
        /// </summary>
        protected virtual void StartMotion()
        {
        }

        /// <summary>
        /// Aca vamos a chequear si la animacion se termino, si el tween finalizo, si el sonido ya termino, si el timer llego a 0
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator CheckPendingRunningMotions()
        {
            yield return null;
        }

        private IEnumerator EndMotionOnButtonPress()
        {
            bool done = false;
            while (!done)
            {
                if (isPerforming == false)
                {
                    done = true;
                    yield break;
                }
                if (Input.GetKeyDown(skipKeyCode))
                {
                    if (isPerforming == true)
                    {
                        isCancel = true;
                        executionState = ANIMOTIONEXECUTIONSTATE.SKIP;
                        done = true;
                        OnMotionSkip();

                        yield break;
                    }
                }
                yield return null;
            }
            yield return null;
        }

        private void OnMotionEnd()
        {
            if (isCancel == false)
            {
                CheckMotionAfterEnd();
                SetNormalSpeedMotion();
                isPerforming = false;
                executionState = ANIMOTIONEXECUTIONSTATE.EXECUTED;
            }
            else
            {
                isCancel = false;
                isPerforming = false;
                executionState = ANIMOTIONEXECUTIONSTATE.EXECUTED;
            }
        }

        protected virtual void CheckMotionAfterEnd()
        {
        }

        public virtual void OnMotionSkip()
        {
            SetNormalSpeedMotion();
            isPerforming = false;
            executionState = ANIMOTIONEXECUTIONSTATE.SKIP;
        }

        #region SPEED UP DOWN MOTION

        public void SpeedUpMotion(float speed)
        {
            this.speed = speed;
            SpeedUpMotionOnMotion();
        }

        protected virtual void SpeedUpMotionOnMotion()
        {
        }
       
        public void SetNormalSpeedMotion()
        {
            SetNormalSpeedInMotion();
        }

        protected virtual void SetNormalSpeedInMotion()
        {
        }

        #endregion
    }
}