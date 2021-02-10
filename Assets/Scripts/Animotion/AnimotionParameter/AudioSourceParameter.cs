namespace MikzeerGame.Animotion
{
    #region ANIMOTION PARAMETERS

    public class AudioSourceParameter
    {
        public bool oneShot { get; private set; }
        public bool isLooping { get; private set; }
        public float oneShotVolume { get; private set; }

        public AudioSourceParameter(bool oneShot = false, bool isLooping = false, float oneShotVolume = 0.7f)
        {
            this.oneShot = oneShot;
            this.isLooping = isLooping;
            this.oneShotVolume = oneShotVolume;
        }
    }

    #endregion
}