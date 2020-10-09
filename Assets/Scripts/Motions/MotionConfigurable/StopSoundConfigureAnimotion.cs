using System;
using UnityEngine;
namespace PositionerDemo
{

    public class StopSoundConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : AudioSourceGenericContainer where O : Transform
    {
        public StopSoundConfigureAnimotion(T firstConfigure, int configureOrder, bool isForced = true) : base(firstConfigure, configureOrder, isForced)
        {
        }

        public override void Configure()
        {
            firstConfigure.audioSource.Stop();
            //Debug.Log("Stop Sound");
        }
    }

    public class AudioSourceGenericContainer
    {
        public AudioSource audioSource;

        public AudioSourceGenericContainer(AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }
    }




}

