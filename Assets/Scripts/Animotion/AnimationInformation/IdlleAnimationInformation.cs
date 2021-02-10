namespace MikzeerGame.Animotion
{
    #region ANIMATION INFORMATION

    public class IdlleAnimationInformation : AnimationInformation
    {
        private const string START_PARAMETER_STRING = "Idlle";
        private const string END_PARAMETER_STRING = "Idlle";
        private const string ANIMATION_STATE_NAME = ".Idlle";
        private const string ANIMATOR_LAYER = "Base Layer";
        private const int ANIMATOR_LAYER_INDEX = 0;

        public IdlleAnimationInformation() : base(START_PARAMETER_STRING, END_PARAMETER_STRING, ANIMATION_STATE_NAME)
        {
        }
    }

    #endregion
}