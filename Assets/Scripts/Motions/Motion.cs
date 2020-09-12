using System.Collections;
using UnityEngine;
namespace PositionerDemo
{

    public abstract class Motion
    {
        private IEnumerator actualMotionCoroutine;
        public MonoBehaviour coroutineMono { get; private set; }
        private bool cancel = false;
        private bool canSkip = true;
        private KeyCode skipKeyCode = KeyCode.Space;
        public bool performing { get; private set; }

        protected float speed;


        // Esto Corresponde solo a las Animaciones
        protected float animationNormalSpeed = 1;
        public float animationSpeedUpVelocity { get; protected set; }
        public string animationSpeedParameter { get; protected set; }

        // Esto corresponde solo a los Tween
        protected float tweenNormalSpeed = 1;
        public float tweenSpeedUp { get; protected set; }
        public float tweenActualSpeed { get; protected set; }


        public Motion(MonoBehaviour coroutineMono)
        {
            this.coroutineMono = coroutineMono;
            animationSpeedParameter = "AnimationSpeed";
        }

        public void CheckMotionBeforeStart()
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

        public virtual bool CheckCorrectInput()
        {
            return true;
        }

        /// <summary>
        /// Esta es la funcion principal que se va a manejar toda la motion que hayamos puestos
        /// </summary>
        /// <returns></returns>
        protected IEnumerator ReproduceMotion()
        {
            performing = true;
            cancel = false;

            StartMotion();

            // Este lo ponemos ya que sino en el primer frame estan todos en Idlle entonces no reconoce el cambio y termina toda la funcion
            yield return null;

            if (canSkip)
            {
                // Esto es para chequear el final de la animacion presionando un boton
                coroutineMono.StartCoroutine(EndMotionOnButtonPress());
            }

            // Esperamos a terminar de hacer la animacion y volver a Iddle tanto de ataque como de recibir dano de los enemigos
            yield return CheckPendingRunningMotions();

            OnMotionEnd();

            yield return null;
        }

        /// <summary>
        /// Aca comenzamos con la motion en si, ya sea un trigger de animacion o un tween
        /// </summary>
        protected virtual void StartMotion()
        {

        }

        /// <summary>
        /// Aca vamos a chequear si la animacion se termino
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator CheckPendingRunningMotions()
        {
            //while (animst.shortNameHash != animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
            //{
            //    //Debug.Log("Wait for End Animation Time Own Animator Check");
            //    yield return null;
            //}

            ////LA FORMA DE HACERLO CON UN STRING
            //while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
            //{
            //    Debug.Log("Wait for End Animation Time Own Animator Check");
            //    yield return null;
            //}

            //for (int i = 0; i < enemies.Count; i++)
            //{
            //    // Tambien esperamos que termine la animacion de Damage de los enemigos
            //    while (!enemies[i].animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
            //    {
            //        //Debug.Log("Wait for End Animation Time Enemies List Check");
            //        yield return null;
            //    }
            //}
            yield return null;
        }

        private IEnumerator EndMotionOnButtonPress()
        {
            bool done = false;
            while (!done)
            {
                if (performing == false)
                {
                    done = true;
                    yield break;
                }
                if (Input.GetKeyDown(skipKeyCode))
                {
                    if (performing == true)
                    {
                        cancel = true;
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
            if (cancel == false)
            {
                CheckMotionAfterEnd();
                SetNormalSpeedMotion();
                performing = false;
                //Debug.Log("Motion Performed and Ended");
            }
            else
            {
                cancel = false;
                performing = false;
            }
        }

        protected virtual void CheckMotionAfterEnd()
        {
            //Debug.Log("Check Motion After End");
        }

        protected virtual void OnMotionSkip()
        {
            SetNormalSpeedMotion();
            performing = false;
            //Debug.Log("Motion Skiped");
        }

        public void SpeedUpMotion(float speed)
        {
            this.speed = speed;

            //tweenSpeedUp = Mathf.FloorToInt(speed * 2);
            //animationSpeedUpVelocity = Mathf.FloorToInt(speed);

            //SpeedUpMotionOnMotion();

            //tweenActualSpeed = tweenSpeedUp;
        }

        protected virtual void SpeedUpMotionOnMotion()
        {

        }
       
        public void SetNormalSpeedMotion()
        {
            SetNormalSpeedInMotion();

            //if (animationSpeedParameter != null)
            //{
            //    SetNormalSpeedInMotion();
            //}

            //tweenActualSpeed = tweenNormalSpeed;
        }

        protected virtual void SetNormalSpeedInMotion()
        {
            Debug.Log("Set Normal Speed Motion");
        }

    }

}

