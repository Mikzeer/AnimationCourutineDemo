using UnityEngine;
using System.Collections;

namespace MikzeerGame.Animotion
{
    public abstract class Animotion
    {
        #region VARIABLES
        // Orden en el que se deberia reproducir
        public int reproductionOrder { get; private set; }
        private IEnumerator actualMotionCoroutine;
        public MonoBehaviour coroutineMono { get; private set; }
        protected float speed;
        public ANIMOTIONEXECUTIONSTATE executionState { get; private set; } = ANIMOTIONEXECUTIONSTATE.WAIT;
        #endregion

        public Animotion(MonoBehaviour coroutineMono, int reproductionOrder)
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
            //isPerforming = true;
            //isCancel = false;

            StartMotion();
            // Este lo ponemos ya que sino en el primer frame estan todos en Idlle entonces no reconoce el cambio y termina toda la funcion
            // TECNICAMENTE DEBERIA ESTAR SOLUCIONADO SI HABILITAMOS EL CODIGO DEL ANIMATED MOTION QUE CHEQUEA ENTRAR EN EL ESTADO ANTES
            // PARA ESO DEBEMOS CORREGIR TANTO LA ANIMATED MOTION COMO TODOS SUS CHILDS PARA QUE TENGAN LA MISMA LOGICA
            // TENER CUIDADO CON IDDLE O TODOS LOS QUE NO REQUIERAN QUE TERMINE POR LAS DUDAS
            yield return null;

            //// Esto es para chequear el final de la animacion/tween/sonido/timer presionando un boton skipKeyCode
            //if (canSkip)
            //{
            //    coroutineMono.StartCoroutine(EndMotionOnButtonPress());
            //}

            // Esperamos a terminar de hacer la animacion y volver a Iddle tanto de ataque como de recibir dano de los enemigos
            yield return CheckPendingRunningMotions();


            // SI LLEGAMOS ACA LA MOTION YA SE FINALIZO, SIEMPRE, SI O SI, YA SEA POR SKIP O POR QUE TERMINO REALMENTE
            // PODRIA PASAR QUE JUSTO LE DIMOS END EN EL MISMO FRAME QUE HABIA TERMINADO DE CHECKEAR Y VA A EJECUTAR EL ON MOTION SKIP
            // HAY QUE VER SI PRIMERO SE DETIENE LA COROUTINE O SI SE EJECUTA LA COROUTINE Y DESPUES EL ON MOTION SKIP
            // PODRIA PONER UN ONMOTIONSKIP ACA PARA VER COMO FUNCIONA
            //OnMotionSkip();
            if (executionState == ANIMOTIONEXECUTIONSTATE.SKIP)
            {
                yield break;
            }

            OnMotionEnd();
            SetNormalSpeedMotion();
            executionState = ANIMOTIONEXECUTIONSTATE.EXECUTED;




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
        /// Al finalizar cada motion se va a poner en state EXECUTED
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator CheckPendingRunningMotions()
        {
            yield return null;
        }

        protected virtual void OnMotionEnd()
        {
        }

        public virtual void OnMotionSkip()
        {
            executionState = ANIMOTIONEXECUTIONSTATE.SKIP;
            //// VER SI LO HACEMOS ASI O NO
            //if (actualMotionCoroutine != null)
            //{
            //    coroutineMono.StopCoroutine(actualMotionCoroutine);
            //}
            SetNormalSpeedMotion();
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