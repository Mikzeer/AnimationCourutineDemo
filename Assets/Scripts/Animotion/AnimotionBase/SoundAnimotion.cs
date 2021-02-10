using UnityEngine;
using System.Collections;

namespace MikzeerGame.Animotion
{
    public class SoundAnimotion : Animotion
    {
        protected float soundNormalSpeed = 1;
        public float soundSpeedUp { get; protected set; }
        public float soundActualSpeed { get; protected set; }
        private float volumen = 1;
        private AudioSource audioSource;
        private AudioClip audioClip;
        private AudioSourceParameter audioSourceParameter;

        public SoundAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, AudioSource audioSource, AudioClip audioClip, AudioSourceParameter audioSourceParameter) : base(coroutineMono, reproductionOrder)
        {
            soundActualSpeed = soundNormalSpeed;
            this.audioSource = audioSource;
            this.audioClip = audioClip;
            this.audioSourceParameter = audioSourceParameter;
        }

        protected override void StartMotion()
        {
            audioSource.volume = volumen;
            audioSource.pitch = soundActualSpeed;

            if (audioSourceParameter.isLooping)
            {
                audioSource.loop = true;
            }
            else
            {
                audioSource.loop = false;
            }
            // PLAY / PLAYONESHOT
            if (audioSourceParameter.oneShot == false)
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
                audioSource.PlayOneShot(audioClip, audioSourceParameter.oneShotVolume);
            }
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            if (audioSourceParameter.oneShot == false)
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