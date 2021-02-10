using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public class StopSoundConfigureContainer : ConfigureContainer
    {
        // StopSoundConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : AudioSourceGenericContainer where O : Transform
        public AudioSource audioSource { get; private set; }

        public StopSoundConfigureContainer(AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }

        public void Execute()
        {
            audioSource.Stop();
        }
    }

    #endregion
}