using System.Collections;
using UnityEngine;
namespace PositionerDemo
{
    public class SoundMotion : Motion
    {
        protected float soundNormalSpeed = 1;
        public float soundSpeedUp { get; protected set; }
        public float soundActualSpeed { get; protected set; }

        private float volumen = 1;

        private AudioSource audioSource;
        private AudioClip audioClip;
        private bool oneShot;
        private bool isLooping;
        private float oneShotVolume;

        public SoundMotion(MonoBehaviour coroutineMono, int reproductionOrder, AudioSource audioSource, AudioClip audioClip, bool oneShot = false, bool isLooping = false, float oneShotVolume = 0.7f) : base(coroutineMono, reproductionOrder)
        {
            soundActualSpeed = soundNormalSpeed;

            this.audioSource = audioSource;
            this.audioClip = audioClip;
            this.oneShot = oneShot;
            this.isLooping = isLooping;
            this.oneShotVolume = oneShotVolume;
        }

        protected override void StartMotion()
        {
            audioSource.volume = volumen;
            audioSource.pitch = soundActualSpeed;

            if (isLooping)
            {
                audioSource.loop = true;
            }
            else
            {
                audioSource.loop = false;
            }
            // PLAY / PLAYONESHOT
            if (oneShot == false)
            {
                // PLAY ES MAS PARA SONIDOS LARGOS O QUE LOOPEAN
                // The standard Play function, however, is better for music, ambience and other effects that 
                // require looping (which you can’t do with PlayOneShot).
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                // ONESHOT ES MAS PARA SONIDOS CORTOS COMO EL DE ATAQUE Y DANO QUE SUENAN Y SE VAN
                //PlayOneShot is best for short, non-looping clips such as sound effects. It’s also useful for
                //triggering multiple sounds from the same Audio Source, without them interrupting each other.
                audioSource.PlayOneShot(audioClip, oneShotVolume);
            }


        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            if (oneShot == false)
            {
                // si el clip es el mismo y esta sonando todavia
                while (audioSource.clip == audioClip && audioSource.isPlaying)
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }

        public override void OnMotionSkip()
        {
            audioSource.Stop();
            base.OnMotionSkip();
        }

        protected override void SpeedUpMotionOnMotion()
        {
            soundSpeedUp = 1.5f;
            soundActualSpeed = soundSpeedUp;
            audioSource.pitch = soundActualSpeed;
        }

        protected override void SetNormalSpeedInMotion()
        {
            soundActualSpeed = soundNormalSpeed;
            audioSource.pitch = soundActualSpeed;
        }

    }
}

